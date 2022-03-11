using System;

namespace Lemoncode.CryptoFiatConverter.Abstractions
{
    public sealed class ConversionResult
    {
        public string CryptoCode { get; }
        public DateTime RateUpdatedOn { get; }
        public decimal TotalEur { get; }

        public ConversionResult(string cryptoCode, DateTime rateUpdatedOn, decimal totalEur)
        {
            CryptoCode = cryptoCode;
            RateUpdatedOn = rateUpdatedOn;
            TotalEur = totalEur;
        }
    }
}
