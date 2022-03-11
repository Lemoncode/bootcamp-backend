using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Lemoncode.CryptoFiatConverter.Contracts;

namespace Lemoncode.CryptoFiatConverter
{
    public class CoinGeckoExchangeRateUpdater
        : IExchangeRateUpdater
    {
        private readonly IPriceDatabase _priceDatabase;

        public CoinGeckoExchangeRateUpdater(IPriceDatabase priceDatabase)
        {
            _priceDatabase = priceDatabase;
        }

        public async Task Update()
        {
            var httpClient = new HttpClient();

            var commaSeparatedCryptoIds = string.Join(",", SupportedCrypto.GetAllIds());
            
            var response = await httpClient.GetAsync(
                $"https://api.coingecko.com/api/v3/simple/price?ids={commaSeparatedCryptoIds}&vs_currencies=EUR");

            var json = await response.Content.ReadAsStringAsync();

            var rates = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(json);
            if (rates is null)
            {
                throw new ApplicationException("Could not retrieve rates from CoinGecko");
            }

            var allPrices = new Dictionary<string, decimal>();

            foreach (var rate in rates)
            {
                var cryptoId = rate.Key;
                var cryptoCode = SupportedCrypto.GetCryptoCode(cryptoId);
                var priceEur = rate.Value["eur"];
                allPrices.Add(cryptoCode, priceEur);
            }

            _priceDatabase.SetAllPrices(allPrices);
        }
    }
}
