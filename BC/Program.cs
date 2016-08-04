using System.IO;

namespace BC
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                var vm = new Vm();
                var buff = File.ReadAllBytes(args[0]);

                vm.LoadMeta(buff);

                vm.InvokeRoot();

                return vm.InvokeMain(args);
            }
            else
            {
                var bc = new ByteCodeWriter();

                //bc.GetRoot().WriteInstruction(Instruction.Print, "Hello From Root Scope").
                //  Flush();

                //main
                var mainPtr = bc.AddMethod(new InstructionSet
                {
                    {
                        Instruction.Branch, true, false, new InstructionSet()
                        {
                            { Instruction.LdS, "Hello if true" },
                            Instruction.Print
                        }
                    },
                    {Instruction.LdI, 8},
                    {Instruction.LdI, 2},
                    { Instruction.LdS, "8 + 2 = " },
                    Instruction.Print,
                    Instruction.AddI,
                    Instruction.Print,
                    Instruction.Pause,
                    {Instruction.Ret, 0}
                }, Primitive.Integer, true).Flush();

                var buff = bc.ToArray();

                File.WriteAllBytes("test.bc", buff);
            }

            return 0;
        }
    }
}