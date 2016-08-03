using System;
using System.Collections;
using System.Collections.Generic;

namespace BC
{
    public class Method
    {
        public readonly Stack Stack = new Stack();
        public byte[] Bc { get; set; }
        public int Count { get; set; }
        public object Ret { get; set; } = new Void();
        public Primitive ReturnType { get; set; }
        public MethodParameter Parameters { get; set; } = MethodParameter.None;
        public List<Local> Args { get; set; } = new List<Local>();
        public Pointer Handle { get; set; }
        public bool IsMain { get; set; }
        public List<Local> Locals { get; } = new List<Local>();

        public override string ToString() =>
            Enum.GetName(typeof (Primitive), ReturnType) + " " + Handle;
    }
}