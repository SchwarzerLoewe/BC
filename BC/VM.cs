﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace BC
{
    public class Vm
    {
        private readonly List<Method> _methods = new List<Method>();

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int system(string command);

        public void LoadMeta(byte[] buf)
        {
            var br = new BinaryReader(new MemoryStream(buf));

            var magic = br.ReadInt32();

            if (magic == 0xF00D)
            {
                var mc = br.ReadInt32();
                for (var i = 0; i < mc; i++)
                {
                    var m = new Method();

                    var fp = br.ReadInt32();
                    m.Handle = Pointer.From(br.ReadBytes(fp));
                    m.IsMain = br.ReadBoolean();
                    m.ReturnType = (Primitive) br.ReadByte();
                    m.Parameters = (MethodParameter) br.ReadByte();
                    m.Count = br.ReadInt32();

                    var raw = br.ReadBytes(m.Count);
                    m.Bc = raw;

                    _methods.Add(m);
                }
            }
            else
            {
                throw new FormatException("Wrong Binary Format!! 0xF00DBA3 needed");
            }

            br.Close();
        }

        public void InvokeRoot()
        {
            Invoke(Pointer.Root);
        }

        public object Invoke(Pointer fp, object[] args = null)
        {
            var m = _methods.FirstOrDefault(tm => tm.Handle == fp);

            return Invoke(m, args);
        }

        public int InvokeMain(object[] args)
        {
            var m = _methods.FirstOrDefault(tm => tm.IsMain);

            if (m != null && m.ReturnType == Primitive.Integer)
            {
                return (int) Invoke(m, args);
            }

            Invoke(m, args);

            return 0;
        }

        public void InvokeScope(int count, Scope s, BinaryReader br, ref Method parentMethod)
        {
            for (var i = 0; i < count; i++)
            {
                var instruction = (Instruction)br.ReadByte();

                switch (instruction)
                {
                    case Instruction.LdI:
                        s.Stack.Push(br.ReadInt32());

                        break;
                    case Instruction.LdF:
                        s.Stack.Push(br.ReadSingle());

                        break;
                    case Instruction.LdB:
                        s.Stack.Push(br.ReadBoolean());

                        break;
                    case Instruction.LdS:
                        s.Stack.Push(new BcString(br.ReadString()).ToReadable());

                        break;
                    case Instruction.AddI:
                        var b = s.Stack.Pop<int>();
                        var a = s.Stack.Pop<int>();

                        s.Stack.Push(a + b);

                        break;
                    case Instruction.Call:
                        var fpC = br.ReadInt32();

                        Invoke(Pointer.From(br.ReadBytes(fpC)));

                        break;
                    case Instruction.Print:
                        Console.Write(s.Stack.Pop());

                        break;
                    case Instruction.Pause:
                        Console.WriteLine();
                        system("pause");

                        break;
                    case Instruction.Local:
                        var fp = br.ReadInt32();
                        var ptr = Pointer.From(br.ReadBytes(fp));
                        var t = (Primitive)br.ReadByte();

                        var v = Primitive.Null;

                        var l = new Local { DataType = t, Handle = ptr, Value = v };

                        s.Locals.Add(l);
                        break;
                    case Instruction.Ret:
                        object obj = GetObject(parentMethod.ReturnType, br);
                        parentMethod.Ret = obj;
                        
                        s.Locals.Clear();
                        Pointer handle = parentMethod.Handle;
                        _methods.Replace(_ => _.Handle == handle, parentMethod);

                        break;
                }
            }
        }

        private object GetObject(Primitive p, BinaryReader br)
        {
            object ret = null;

            switch (p)
            {
                case Primitive.Integer:
                    ret = br.ReadInt32();
                    break;
                case Primitive.Float:
                    ret = br.ReadSingle();
                    break;
                case Primitive.Bool:
                    ret = br.ReadBoolean();
                    break;
                case Primitive.String:
                    ret = new BcString(br.ReadString()).ToReadable();
                    break;
            }

            return ret;
        }

        public object Invoke(Method m, object[] args = null)
        {
            if (m != null)
            {
                var br = new BinaryReader(new MemoryStream(m.Bc));

                m.Args = ConvertArgsToLocal(args);

                var c = br.ReadInt32();
                InvokeScope(c, m.Scope, br, ref m);

                br.Close();
            }

            return m?.Ret;
        }

        private List<Local> ConvertArgsToLocal(object[] args)
        {
            var ret = new List<Local>();

            if (args != null)
            {
                foreach (var arg in args)
                {
                    var l = new Local();
                    l.DataType = l.DataType.GetPrimitiveFor(arg);
                    l.Value = arg;

                    ret.Add(l);
                }
            }

            return ret;
        }
    }
}