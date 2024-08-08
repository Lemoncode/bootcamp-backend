using Lemoncode.CryptoFiatConverter.Contracts;
using Lemoncode.CryptoFiatConverter.Factories;

namespace Lemoncode.CryptoFiatConverter.UnitTests.TestSupport.Builders
{
    public class InMemoryPriceDatabaseBuilder
    {
        private IDateTimeFactory _dateTimeFactory;

        public InMemoryPriceDatabaseBuilder()
        {
            _dateTimeFactory = new DateTimeFactory();
        }

        public InMemoryPriceDatabaseBuilder WithDateTimeFactory(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
            return this;
        }

        public InMemoryPriceDatabase Build()
        {
            var result = new InMemoryPriceDatabase(_dateTimeFactory);
            return result;
        }
    }
}
