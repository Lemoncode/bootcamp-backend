using System;
using System.Collections.Generic;
using Lemoncode.CryptoFiatConverter.Contracts;
using Lemoncode.CryptoFiatConverter.Models;

namespace Lemoncode.CryptoFiatConverter
{
    public class InMemoryPriceDatabase
        : IPriceDatabase
    {
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly IDictionary<string, decimal> _prices = new Dictionary<string, decimal>();
        private DateTime _lastUpdatedOn = DateTime.MinValue;

        public InMemoryPriceDatabase(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
        }


        public CurrentPrice GetPrice(string cryptoCode)
        {
            var isStored = _prices.TryGetValue(cryptoCode, out var price);
            if(!isStored)
            {
                throw new KeyNotFoundException($"Key {cryptoCode} does not exist");
            }

            var currentPrice = new CurrentPrice(cryptoCode, _lastUpdatedOn, price);
            return currentPrice;
        }

        public void SetAllPrices(IDictionary<string, decimal> prices)
        {
            foreach (var newPrice in prices)
            {
                _prices[newPrice.Key] = newPrice.Value;
            }

            _lastUpdatedOn = _dateTimeFactory.GetCurrentUtc();
        }
    }
}
