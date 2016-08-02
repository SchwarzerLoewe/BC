using System;
using System.Collections.Generic;
using System.IO;

namespace BC
{
    public class ByteCodeWriter
    {
        readonly MemoryStream _ms = new MemoryStream();

        Dictionary<Guid, Method> Methods { get; set; } =
            new Dictionary<Guid, Method>();

        Dictionary<Guid, InstructionWriter> Writers { get; set; } =
            new Dictionary<Guid, InstructionWriter>();

        public InstructionWriter AddMethod(Primitive prim = Primitive.Void)
        {
            var iw = new InstructionWriter(Pointer.NewFp());

            Methods.Add(iw.Handle, new Method {Handle = iw.Handle, ReturnType = prim});
            Writers.Add(iw.Handle, iw);

            return iw;
        }

        public InstructionWriter AddMethod(InstructionSet set, Primitive prim = Primitive.Void, bool isMain = false)
        {
            var iw = set.GetWriter();

            Methods.Add(iw.Handle, new Method {Handle = iw.Handle, ReturnType = prim, IsMain = isMain});
            Writers.Add(iw.Handle, iw);

            return iw;
        }

        public InstructionWriter GetRoot()
        {
            var iw = new InstructionWriter(Pointer.Root);

            Methods.Add(Pointer.Root, new Method {Handle = Pointer.Root, ReturnType = Primitive.Void});
            Writers.Add(Pointer.Root, iw);

            return iw;
        }

        public byte[] ToArray()
        {
            var bw = new BinaryWriter(_ms);
            bw.Write(0xF00D);

            bw.Write(Methods.Count);
            foreach (var m in Writers)
            {
                // functionpointer
                bw.Write(m.Key.ToByteArray().Length);
                bw.Write(m.Key.ToByteArray());

                bw.Write(Methods[m.Key].IsMain);
                bw.Write((byte) Methods[m.Key].ReturnType);
                bw.Write((byte) Methods[m.Key].Parameters);

                var buf = m.Value.ToArray();

                bw.Write(buf.Length);
                bw.Write(buf);
            }

            return _ms.ToArray();
        }
    }
}