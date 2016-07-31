using System.IO;

namespace BC
{
    class Program
    {
        static int Main(string[] args)
        {
            var bc = new ByteCodeWriter();

            bc.GetRoot().WriteInstruction(Instruction.print, "Hello From Root Scope").
                Flush();

            //test
            var iwTest = bc.AddMethod(Primitive.String);
            var testPtr = iwTest.WriteInstruction(Instruction.print, "Hello World").
            WriteInstruction(Instruction.pause).
            WriteInstruction(Instruction.ret, "FooBar").
            Flush();

            var tptr = bc.AddMethod(new InstructionSet
            {
                { Instruction.print, "Hello World" },
                { Instruction.pause },
                { Instruction.ret, "FooBar" }
            }).Flush();

            //main
            var iw = bc.AddMethod(Primitive.Integer);

            var mainPtr = iw.WriteInstruction(Instruction.ld_i, 3).
            WriteInstruction(Instruction.ld_i, 3).
            WriteInstruction(Instruction.add_i).
            WriteInstruction(Instruction.call, tptr).
            WriteInstruction(Instruction.ret, 0).
            Flush();

            var vm = new VM();
            var buff = bc.ToArray();

            File.WriteAllBytes("test.bc", buff);

            vm.LoadMeta(buff);

            vm.InvokeRoot();
            
            return (int)vm.Invoke(mainPtr, args);
        }
    }
}