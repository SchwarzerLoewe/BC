using System;
using System.IO;
using System.Runtime.InteropServices;

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
            var tptr = bc.AddMethod(new InstructionSet
            {
                { Instruction.print, "Hello World" },
                { Instruction.pause },
                { Instruction.ret, "FooBar" }
            }, Primitive.String).Flush();

            //main
            var mainPtr = bc.AddMethod(new InstructionSet
            {
                { Instruction.ld_i, 3},
                { Instruction.ld_i, 3},
                { Instruction.add_i },
                { Instruction.call, tptr },
                { Instruction.ret, 0 }
            }, Primitive.Integer).Flush();

            var vm = new VM();
            var buff = bc.ToArray();

            File.WriteAllBytes("test.bc", buff);

            vm.LoadMeta(buff);

            vm.InvokeRoot();
            
            return (int)vm.Invoke(mainPtr, args);
        }
    }
}