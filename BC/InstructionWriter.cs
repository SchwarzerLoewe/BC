using System;
using System.IO;

namespace BC
{
    public class InstructionWriter
    {
        BinaryWriter bw;
        int count;
        MemoryStream ms;
        internal FunctionPointer Handle;

        public InstructionWriter(FunctionPointer fp)
        {
            ms = new MemoryStream();
            bw = new BinaryWriter(ms);

            Handle = fp;
        }

        public InstructionWriter WriteInstruction(Instruction i, int val)
        {
            count++;

            bw.Write((byte)i);
            bw.Write(val);

            return Write();
        }
        public InstructionWriter WriteInstruction(Instruction i, FunctionPointer val)
        {
            count++;

            bw.Write((byte)i);

            var buf = val.ToArray();

            bw.Write(buf.Length);
            bw.Write(buf);

            return Write();
        }
        public InstructionWriter WriteInstruction(Instruction i, float val)
        {
            count++;

            bw.Write((byte)i);
            bw.Write(val);

            return Write();
        }
        public InstructionWriter WriteInstruction(Instruction i, BCString val)
        {
            count++;

            bw.Write((byte)i);
            bw.Write(val.ToString());

            return Write();
        }
        public InstructionWriter WriteInstruction(Instruction i)
        {
            count++;

            bw.Write((byte)i);

            return Write();
        }

        InstructionWriter Write()
        {
            IsEmpty = false;

            return this;
        }

        public bool IsFlushed { get; set; }
        public bool IsEmpty { get; set; }

        public FunctionPointer Flush()
        {
            if (!IsFlushed)
            {
                bw.Flush();

                var tba = new byte[ms.Length + 4];
                Array.Copy(ms.ToArray(), 0, tba, 4, ms.Length);

                var tms = new MemoryStream(tba);
                var tbw = new BinaryWriter(tms);
                tbw.Write(count);
                tbw.Flush();
                tbw.Close();

                ms = tms;
                bw = tbw;

                IsFlushed = true;
            }

            return Handle;
        }

        public byte[] ToArray()
        {
            return ms.ToArray();
        }
    }
}