using System.Text;

namespace BC
{
    public class BcString
    {
        readonly string _b;

        public BcString(string ba)
        {
            _b = ba;
        }

        public string ToReadable() => Encoding.ASCII.GetString(Base58Encoding.Decode(_b));

        public static implicit operator BcString(string b) => new BcString(b);

        public override string ToString() => Base58Encoding.Encode(Encoding.ASCII.GetBytes(_b));
    }
}