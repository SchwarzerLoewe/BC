using System.Collections;
using System.Collections.Generic;

namespace BC
{
    public class InstructionSet : IEnumerable<byte>
    {
        private readonly InstructionWriter _writer;

        public InstructionSet()
        {
            _writer = new InstructionWriter(Pointer.NewFp());
        }

        public IEnumerator<byte> GetEnumerator() => null;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Instruction x, BcString s)
        {
            _writer.WriteInstruction(x, s);
        }

        public void Add(Instruction x, int s)
        {
            _writer.WriteInstruction(x, s);
        }
        public void Add(Instruction x, object l, object r, InstructionSet trueSet, InstructionSet falseSet = null)
        {
            _writer.WriteInstruction(x);
            _writer.WriteObject(l);
            _writer.WriteObject(r);

            _writer._bw.Write(falseSet != null);
            var tbuffer = trueSet._writer.ToArray();

            _writer._bw.Write(tbuffer.Length);
            _writer._bw.Write(tbuffer);

            if (falseSet != null)
            {
               //ToDO: write false set
            }
        }

        public void Add(Instruction x, float s)
        {
            _writer.WriteInstruction(x, s);
        }

        public void Add(Instruction x, bool s)
        {
            _writer.WriteInstruction(x, s);
        }

        public void Add(Instruction x, Pointer fptr)
        {
            _writer.WriteInstruction(x, fptr);
        }

        public void Add(Instruction x, Pointer fptr, object val)
        {
            _writer.WriteInstruction(x, fptr, val);
        }

        public void Add(Instruction x)
        {
            _writer.WriteInstruction(x);
        }

        public InstructionWriter GetWriter() => _writer;
    }
}