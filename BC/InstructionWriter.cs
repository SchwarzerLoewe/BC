using System;
using System.IO;

namespace BC
{
    public class InstructionWriter
    {
        private BinaryWriter _bw;
        private int _count;
        private MemoryStream _ms;
        internal readonly Pointer Handle;

        public InstructionWriter(Pointer fp)
        {
            _ms = new MemoryStream();
            _bw = new BinaryWriter(_ms);

            Handle = fp;
        }

        public bool IsFlushed { get; set; }
        public bool IsEmpty { get; set; }

        public InstructionWriter WriteInstruction(Instruction i, int val)
        {
            _count++;

            _bw.Write((byte) i);
            _bw.Write(val);

            return Write();
        }

        public InstructionWriter WriteInstruction(Instruction i, bool val)
        {
            _count++;

            _bw.Write((byte) i);
            _bw.Write(val);

            return Write();
        }

        public InstructionWriter WriteInstruction(Instruction i, Pointer val)
        {
            _count++;

            _bw.Write((byte) i);

            var buf = val.ToArray();

            _bw.Write(buf.Length);
            _bw.Write(buf);

            return Write();
        }

        public InstructionWriter WriteInstruction(Instruction i, Pointer val, object v)
        {
            _count++;

            _bw.Write((byte) i);

            var buf = val.ToArray();

            _bw.Write(buf.Length);
            _bw.Write(buf);
            // bw.Write(v);

            return Write();
        }

        public InstructionWriter WriteInstruction(Instruction i, float val)
        {
            _count++;

            _bw.Write((byte) i);
            _bw.Write(val);

            return Write();
        }

        public InstructionWriter WriteInstruction(Instruction i, BcString val)
        {
            _count++;

            _bw.Write((byte) i);
            _bw.Write(val.ToString());

            return Write();
        }

        public InstructionWriter WriteInstruction(Instruction i)
        {
            _count++;

            _bw.Write((byte) i);

            return Write();
        }

        private InstructionWriter Write()
        {
            IsEmpty = false;

            return this;
        }

        public Pointer Flush()
        {
            if (!IsFlushed)
            {
                _bw.Flush();

                var tba = new byte[_ms.Length + 4];
                Array.Copy(_ms.ToArray(), 0, tba, 4, _ms.Length);

                var tms = new MemoryStream(tba);
                var tbw = new BinaryWriter(tms);
                tbw.Write(_count);
                tbw.Flush();
                tbw.Close();

                _ms = tms;
                _bw = tbw;

                IsFlushed = true;
            }

            return Handle;
        }

        public byte[] ToArray()
        {
            return _ms.ToArray();
        }
    }
}