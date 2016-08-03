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

        public IEnumerator<byte> GetEnumerator() => new byte[12].GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Instruction x, BcString s)
        {
            _writer.WriteInstruction(x, s);
        }

        public void Add(Instruction x, int s)
        {
            _writer.WriteInstruction(x, s);
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