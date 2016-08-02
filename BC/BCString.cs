using System.Text;

namespace BC
{
    public class BcString
    {
        private readonly string _b;

        public BcString(string ba)
        {
            _b = ba;
        }

        public string ToReadable()
        {
            return Encoding.ASCII.GetString(Base58Encoding.Decode(_b));
        }

        public static implicit operator BcString(string b)
        {
            return new BcString(b);
        }

        public override string ToString()
        {
            return Base58Encoding.Encode(Encoding.ASCII.GetBytes(_b));
        }
    }
}