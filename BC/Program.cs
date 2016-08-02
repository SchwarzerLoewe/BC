using System.IO;

namespace BC
{
    public class Program
    {
        static int Main(string[] args)
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

                bc.GetRoot().WriteInstruction(Instruction.Print, "Hello From Root Scope").
                    Flush();

                //test
                var tptr = bc.AddMethod(new InstructionSet
                {
                    {Instruction.Print, "Hello World"},
                    Instruction.Pause,
                    {Instruction.Ret, "FooBar"}
                }, Primitive.String).Flush();

                //main
                var mainPtr = bc.AddMethod(new InstructionSet
                {
                    {Instruction.LdI, 3},
                    {Instruction.LdI, 3},
                    Instruction.AddI,
                    {Instruction.LdB, true},
                    {Instruction.Call, tptr},
                    {Instruction.Ret, 0}
                }, Primitive.Integer, true).Flush();

                var buff = bc.ToArray();

                File.WriteAllBytes("test.bc", buff);
            }

            return 0;
        }
    }
}