using System.Collections;
using System.Collections.Generic;

namespace BC
{
    public class InstructionSet : IEnumerable<byte>
    {
        private InstructionWriter writer;

        public InstructionSet()
        {
            writer = new InstructionWriter(FunctionPointer.NewFP());
        }

        public void Add(Instruction x, string s)
        {
            writer.WriteInstruction(x, s);
        }
        public void Add(Instruction x, int s)
        {
            writer.WriteInstruction(x, s);
        }
        public void Add(Instruction x, float s)
        {
            writer.WriteInstruction(x, s);
        }
        public void Add(Instruction x, FunctionPointer fptr)
        {
            writer.WriteInstruction(x, fptr);
        }
        public void Add(Instruction x)
        {
            writer.WriteInstruction(x);
        }

        public InstructionWriter GetWriter() => writer;

        public IEnumerator<byte> GetEnumerator() => null;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}