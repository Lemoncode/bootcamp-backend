using System;
using Lemoncode.CryptoFiatConverter.Contracts;

namespace Lemoncode.CryptoFiatConverter.Factories
{
    public class DateTimeFactory
        : IDateTimeFactory
    {
        public DateTime GetCurrentUtc()
        {
            return DateTime.UtcNow;
        }
    }
}
