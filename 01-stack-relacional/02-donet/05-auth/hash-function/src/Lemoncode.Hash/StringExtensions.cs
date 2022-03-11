using System.Security.Cryptography;
using System.Text;

namespace Lemoncode.Hash
{
    public static class StringExtensions
    {
        public static string ToSha256(this string input)
        {
            var inputAsBytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA256.Create();
            var bytes = hash.ComputeHash(inputAsBytes);

            var readableStringBuilder = new StringBuilder();
            foreach (var hashedByte in bytes)
            {
                readableStringBuilder.Append(hashedByte.ToString("X2"));
            }

            var output = readableStringBuilder.ToString();
            return output;
        }
    }
}
