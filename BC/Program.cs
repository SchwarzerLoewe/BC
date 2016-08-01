using System.IO;
using System.Linq;

namespace BC
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                var vm = new VM();
                var buff = File.ReadAllBytes(args[0]);

                vm.LoadMeta(buff);

                vm.InvokeRoot();

                return vm.InvokeMain(args);
            }
            else
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
            }, Primitive.Integer, true).Flush();

                var buff = bc.ToArray();

                File.WriteAllBytes("test.bc", buff);
            }

            return 0;
        }
    }
}