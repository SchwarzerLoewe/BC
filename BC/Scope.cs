using System.Collections;
using System.Collections.Generic;

namespace BC
{
    public class Scope
    {
        public readonly Stack Stack = new Stack();
        public List<Local> Locals { get; } = new List<Local>();
    }
}