using System.Text;

namespace BC
{
    public class BCString
    {
        private string b;
        public BCString(string ba)
        {
            b = ba;
        }

        public string ToReadable()
        {
            return Encoding.ASCII.GetString(Base58Encoding.Decode(b));
        }

        public static implicit operator BCString(string b)
        {
            return new BCString(b);
        }

        public override string ToString()
        {
            return Base58Encoding.Encode(Encoding.ASCII.GetBytes(b));
        }
    }
}