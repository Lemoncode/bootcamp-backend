using System;
using System.Collections.Generic;
using FluentAssertions;
using Lemoncode.CryptoFiatConverter.Contracts;
using Lemoncode.CryptoFiatConverter.Models;
using Lemoncode.CryptoFiatConverter.UnitTests.TestSupport.Mocks;
using Moq;
using Xunit;

namespace Lemoncode.CryptoFiatConverter.UnitTests.InMemoryPriceDatabaseTests
{
    public class GetPriceTests
    {
        [Fact]
        public void Given_Non_Existent_Crypto_Code_When_Getting_Price_Throws_KeyNotFoundException()
        {
            // Given
            var sampleDateTime = new DateTime(2021, 10, 1, 0, 0, 0, DateTimeKind.Utc);
            var sut = new InMemoryPriceDatabase(new DateTimeManualMock(sampleDateTime));
            KeyNotFoundException exception = null;

            // When
            try
            {
                sut.GetPrice("BTC");
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                exception = keyNotFoundException;
            }

            // Then
            exception.Should().NotBeNull();
        }

        [Fact]
        public void Given_Existent_Crypto_Code_When_Getting_Price_Returns_Expected_Current_Price_Details()
        {
            // Given
            var sampleDateTime = new DateTime(2021, 10, 1, 0, 0, 0, DateTimeKind.Utc);
            var sut = new InMemoryPriceDatabase(new DateTimeManualMock(sampleDateTime));
            sut.SetAllPrices(
                new Dictionary<string, decimal>
                {
                    {"BTC", 0.1m}
                });

            // When
            var result = sut.GetPrice("BTC");

            // Then
            result.CryptoCode.Should().Be("BTC");
            result.PriceInEur.Should().Be(0.1m);
            result.LastUpdatedOn.Should().BeSameDateAs(sampleDateTime);
        }

        [Fact]
        public void Given_Existent_Crypto_Code_When_Getting_Price_Returns_Expected_Current_Price_Details_With_Moq()
        {
            // Given
            var sampleDateTime = new DateTime(2021, 10, 1, 0, 0, 0, DateTimeKind.Utc);

            var dateTimeFactoryMock = new Mock<IDateTimeFactory>();
            dateTimeFactoryMock
                .Setup(x => x.GetCurrentUtc())
                .Returns(sampleDateTime);
            var dateTimeFactory = dateTimeFactoryMock.Object;
            
            var sut = new InMemoryPriceDatabase(dateTimeFactory);
            sut.SetAllPrices(
                new Dictionary<string, decimal>
                {
                    {"BTC", 0.1m}
                });

            // When
            var result = sut.GetPrice("BTC");

            // Then
            result.CryptoCode.Should().Be("BTC");
            result.PriceInEur.Should().Be(0.1m);
            result.LastUpdatedOn.Should().BeSameDateAs(sampleDateTime);

            // Assert the result at once
            var expectedResult = new CurrentPrice("BTC", sampleDateTime, 0.1m);
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}