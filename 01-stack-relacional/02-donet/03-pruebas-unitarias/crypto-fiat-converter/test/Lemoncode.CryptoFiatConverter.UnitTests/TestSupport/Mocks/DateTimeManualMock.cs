using System;
using Lemoncode.CryptoFiatConverter.Contracts;

namespace Lemoncode.CryptoFiatConverter.UnitTests.TestSupport.Mocks
{
    public class DateTimeManualMock
        : IDateTimeFactory
    {
        private readonly DateTime _dateTimeToReturn;

        public DateTimeManualMock(DateTime dateTimeToReturn)
        {
            if (dateTimeToReturn.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Only UTC DateTime allowed on this mock");
            }
            _dateTimeToReturn = dateTimeToReturn;
        }

        public DateTime GetCurrentUtc()
        {
            return _dateTimeToReturn;
        }
    }
}
