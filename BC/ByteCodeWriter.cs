using System;
using System.Collections.Generic;
using System.IO;

namespace BC
{
    public class ByteCodeWriter
    {
        public Dictionary<Guid, Method> Methods { get; set; } = 
            new Dictionary<Guid, Method>();
        public Dictionary<Guid, InstructionWriter> Writers { get; set; } =
            new Dictionary<Guid, InstructionWriter>();
        
        MemoryStream ms = new MemoryStream();

        public InstructionWriter AddMethod(Primitive prim = Primitive.Void)
        {
            var iw = new InstructionWriter(FunctionPointer.NewFP());

            Methods.Add(iw.Handle, new Method { Handle = iw.Handle, ReturnType = prim });
            Writers.Add(iw.Handle, iw);

            return iw;
        }

        public InstructionWriter GetRoot()
        {
            var iw = new InstructionWriter(FunctionPointer.Root);

            Methods.Add(FunctionPointer.Root, new Method { Handle = FunctionPointer.Root, ReturnType = Primitive.Void });
            Writers.Add(FunctionPointer.Root, iw);

            return iw;
        }

        public byte[] ToArray()
        {
            var bw = new BinaryWriter(ms);
            bw.Write(0xF00D);

            bw.Write(Methods.Count);
            foreach (var m in Writers)
            {
                // functionpointer
                bw.Write(m.Key.ToByteArray().Length);
                bw.Write(m.Key.ToByteArray());

                bw.Write((byte)Methods[m.Key].ReturnType);
                bw.Write((byte)Methods[m.Key].Parameters);

                var buf = m.Value.ToArray();

                bw.Write(buf.Length);
                bw.Write(buf);
            }

            return ms.ToArray();
        }
    }
}