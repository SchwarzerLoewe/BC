using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace BC
{
    public class VM
    {
        public List<Method> Methods = new List<Method>();

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        static extern int system(string command);

        public void LoadMeta(byte[] buf)
        {
            var br = new BinaryReader(new MemoryStream(buf));

            var magic = br.ReadInt32();

            if (magic == 0xF00D)
            {
                var mc = br.ReadInt32();
                for (int i = 0; i < mc; i++)
                {
                    var m = new Method();

                    var fp = br.ReadInt32();
                    m.Handle = FunctionPointer.From(br.ReadBytes(fp));
                    m.ReturnType = (Primitive)br.ReadByte();
                    m.Parameters = (MethodParameter)br.ReadByte();
                    m.Count = br.ReadInt32();

                    var raw = br.ReadBytes(m.Count);
                    m.BC = raw;

                    Methods.Add(m);
                }
            }
            else
            {
                throw new Exception("Wrong Binary Format!! 0xF00DBA3 needed");
            }

            br.Close();
        }

        public void InvokeRoot()
        {
            Invoke(FunctionPointer.Root);
        }

        public object Invoke(FunctionPointer fp, object[] args = null)
        {
            Method m = null;
            foreach (var tm in Methods)
            {
                if(tm.Handle == fp)
                {
                    m = tm;
                    break;
                }
            }

            return Invoke(m, args);
        }

        public object Invoke(Method m, object[] args = null)
        {
            var br = new BinaryReader(new MemoryStream(m.BC));

            m.Args = ConvertArgsToLocal(args);

            var count = br.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var instruction = (Instruction)br.ReadByte();

                switch (instruction)
                {
                    case Instruction.ld_i:
                        m.stack.Push(br.ReadInt32());

                        break;
                    case Instruction.ld_f:
                        m.stack.Push(br.ReadSingle());

                        break;
                    case Instruction.ld_s:
                        m.stack.Push(br.ReadString());

                        break;
                    case Instruction.add_i:
                        int b = m.stack.Pop<int>();
                        int a = m.stack.Pop<int>();

                        m.stack.Push(a + b);

                        break;
                    case Instruction.call:
                        var fpC = br.ReadInt32();

                        Invoke(FunctionPointer.From(br.ReadBytes(fpC)));

                        break;
                    case Instruction.print:
                        var arg = br.ReadString();

                        Console.WriteLine(arg);

                        break;
                    case Instruction.pause:
                        system("pause");

                        break;
                    case Instruction.ret:
                        switch (m.ReturnType)
                        {
                            case Primitive.Integer:
                                m.Ret = br.ReadInt32();
                                break;
                            case Primitive.Float:
                                m.Ret = br.ReadSingle();
                                break;
                            case Primitive.String:
                                m.Ret = br.ReadString();

                                break;
                        }

                        Methods.Replace((_) => _.Handle == m.Handle, m);

                        break;
                }
            }

            br.Close();

            return m.Ret;
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