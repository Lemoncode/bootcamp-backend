using System.Threading.Tasks;
using FluentAssertions;
using Lemoncode.CryptoFiatConverter.Factories;
using Xunit;

namespace Lemoncode.CryptoFiatConverter.UnitTests.CoinGeckoExchangeRateUpdaterTests
{
    public class UpdateTests
    {
        [Fact]
        public async Task Deserialize()
        {
            var database = new InMemoryPriceDatabase(new DateTimeFactory());
            var updater = new CoinGeckoExchangeRateUpdater(database);
            await updater.Update();

            var result = database.GetPrice("BTC");
            result.PriceInEur.Should().BeGreaterThan(0);
        }
    }
}