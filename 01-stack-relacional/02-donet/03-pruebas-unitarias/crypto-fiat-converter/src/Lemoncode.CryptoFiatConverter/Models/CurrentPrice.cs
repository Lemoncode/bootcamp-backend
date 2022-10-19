using System;

namespace Lemoncode.CryptoFiatConverter.Models
{
    public class CurrentPrice
    {
        public string CryptoCode { get; }
        public DateTime LastUpdatedOn { get; }
        public decimal PriceInEur { get; }

        public CurrentPrice(string cryptoCode, DateTime lastUpdatedOn, decimal priceInEur)
        {
            CryptoCode = cryptoCode;
            LastUpdatedOn = lastUpdatedOn;
            PriceInEur = priceInEur;
        }
    }
}
