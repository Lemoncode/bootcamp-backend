using System;
using System.Threading.Tasks;
using Lemoncode.CryptoFiatConverter.Abstractions;
using Lemoncode.CryptoFiatConverter.Contracts;
using Lemoncode.CryptoFiatConverter.Factories;

namespace Lemoncode.CryptoFiatConverter.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dateTimeFactory = new DateTimeFactory();
            var priceDatabase = new InMemoryPriceDatabase(dateTimeFactory);


            // admin part
            IExchangeRateUpdater updater = new CoinGeckoExchangeRateUpdater(priceDatabase);
            await updater.Update();

            // user part
            IConverter converter = new Converter(priceDatabase);
            var amount = 15000;
            var result = converter.ConvertToEur("HBAR", amount);
            Console.WriteLine($"Result of {amount} HBAR: {result.TotalEur} EUR, (last updated on {result.RateUpdatedOn})");
        }
    }
}
