using System;
using Lemoncode.CryptoFiatConverter.Abstractions;
using Lemoncode.CryptoFiatConverter.Contracts;

namespace Lemoncode.CryptoFiatConverter
{
    public class Converter
        : IConverter
    {
        private readonly IPriceDatabase _priceDatabase;

        public Converter(IPriceDatabase priceDatabase)
        {
            _priceDatabase = priceDatabase;
        }

        public ConversionResult ConvertToEur(string cryptoCode, decimal amount)
        {
            var isSupported = SupportedCrypto.IsSupported(cryptoCode);
            if (!isSupported)
            {
                throw new ArgumentException($"Crypto code {cryptoCode} is not supported");
            }

            var currentPrice = _priceDatabase.GetPrice(cryptoCode);

            var total = amount * currentPrice.PriceInEur;

            var result = new ConversionResult(currentPrice.CryptoCode, currentPrice.LastUpdatedOn, total);
            return result;
        }
    }
}
