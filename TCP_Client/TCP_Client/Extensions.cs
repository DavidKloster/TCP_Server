using System.Text;

namespace TCP_Client
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this string s)
        {
            var asciiEncoding = new ASCIIEncoding();
            return asciiEncoding.GetBytes(s);
        }
    }
}