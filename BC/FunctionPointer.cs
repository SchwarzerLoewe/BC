using System;

namespace BC
{
    public class Pointer
    {
        private Guid Id { get; set; }

        public static Pointer Root { get; set; } = From(Guid.Parse("8cb0177e-8f1c-4a25-a5ce-e47754e7684a"));

        public static Pointer NewFp() =>
            new Pointer {Id = Guid.NewGuid()};

        public byte[] ToArray() => Id.ToByteArray();

        public static Pointer From(byte[] raw) =>
            new Pointer {Id = new Guid(raw)};

        public static Pointer From(Guid g) =>
            new Pointer {Id = g};

        public override string ToString()
        {
            var buf = Id.ToByteArray();
            var first = BitConverter.ToUInt64(buf, 0);
            var second = BitConverter.ToUInt64(buf, 8);

            return $"[{first}, {second}]";
        }

        public static bool operator ==(Pointer a, Pointer b) => a.Id == b.Id;
        public static bool operator !=(Pointer a, Pointer b) => a.Id != b.Id;

        public override bool Equals(object obj) => Id.Equals(obj);
        public override int GetHashCode() => Id.GetHashCode();

        public static implicit operator Guid(Pointer fp) => fp.Id;
        public static implicit operator Pointer(Guid fp) => new Pointer {Id = fp};
    }
}