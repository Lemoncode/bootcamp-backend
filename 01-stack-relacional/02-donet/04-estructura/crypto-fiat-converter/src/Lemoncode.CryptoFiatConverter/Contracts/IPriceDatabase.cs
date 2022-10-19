using System.Collections.Generic;
using Lemoncode.CryptoFiatConverter.Models;

namespace Lemoncode.CryptoFiatConverter.Contracts
{
    public interface IPriceDatabase
    {
        CurrentPrice GetPrice(string cryptoCode);

        void SetAllPrices(IDictionary<string, decimal> prices);
    }
}