using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.CryptoFiatConverter
{
    internal static class SupportedCrypto
    {
        private static readonly IDictionary<string, string> SupportedCoins
            = new Dictionary<string, string>
            {
                { "HBAR", "hedera-hashgraph" },
                { "BTC", "bitcoin" },
                { "ETH", "ethereum" },
            };

        public static bool IsSupported(string cryptoCode)
        {
            return SupportedCoins.ContainsKey(cryptoCode);
        }

        public static string GetCryptoCode(string id)
        {
            var code = SupportedCoins.First(x => x.Value == id).Key;
            return code;
        }

        public static IEnumerable<string> GetAllIds()
        {
            return SupportedCoins.Values.ToArray();
        }
    }
}
