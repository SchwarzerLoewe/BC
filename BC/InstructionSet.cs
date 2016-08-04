using System.Collections;
using System.Collections.Generic;

namespace BC
{
    public class InstructionSet : IEnumerable<byte>
    {
        private readonly InstructionWriter _writer;
        private ByteCodeWriter _m;

        public InstructionSet(ByteCodeWriter m)
        {
            _writer = new InstructionWriter(Pointer.NewFp());
            _m = m;
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
            var branchTrueMethodPtr = _m.AddMethod(trueSet).Handle;
            
            _writer.WriteInstruction(Instruction.Call, branchTrueMethodPtr);

            if (falseSet != null)
            {
                var branchFalseMethodPtr = _m.AddMethod(falseSet).Handle;

                _writer.WriteInstruction(Instruction.Call, branchFalseMethodPtr);
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