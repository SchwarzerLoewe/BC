using System;

namespace BC
{
    public class FunctionPointer
    {
        private Guid ID { get; set; }

        public static FunctionPointer Root { get; set; } = From(Guid.Parse("8cb0177e-8f1c-4a25-a5ce-e47754e7684a"));

        public static FunctionPointer NewFP() =>
            new FunctionPointer { ID = Guid.NewGuid() };

        public byte[] ToArray() => ID.ToByteArray();
        public static FunctionPointer From(byte[] raw) =>
            new FunctionPointer { ID = new Guid(raw) };
        public static FunctionPointer From(Guid g) =>
            new FunctionPointer { ID = g };

        public override string ToString() => ID.ToString();

        public static bool operator ==(FunctionPointer a, FunctionPointer b) => a.ID == b.ID;
        public static bool operator !=(FunctionPointer a, FunctionPointer b) => a.ID != b.ID;

        public override bool Equals(object obj) => ID.Equals(obj);
        public override int GetHashCode() => ID.GetHashCode();

        public static implicit operator Guid(FunctionPointer fp) => fp.ID;
        public static implicit operator FunctionPointer(Guid fp) => new FunctionPointer { ID =  fp };
    }
}