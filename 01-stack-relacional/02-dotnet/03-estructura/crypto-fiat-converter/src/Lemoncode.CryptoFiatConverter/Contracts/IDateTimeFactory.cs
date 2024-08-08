using System;

namespace Lemoncode.CryptoFiatConverter.Contracts
{
    public interface IDateTimeFactory
    {
        DateTime GetCurrentUtc();
    }
}