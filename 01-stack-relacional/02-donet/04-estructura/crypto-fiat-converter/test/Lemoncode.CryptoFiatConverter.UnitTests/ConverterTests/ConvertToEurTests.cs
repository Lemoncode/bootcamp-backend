using System;
using System.Collections.Generic;
using FluentAssertions;
using Lemoncode.CryptoFiatConverter.Abstractions;
using Lemoncode.CryptoFiatConverter.Contracts;
using Moq;
using Xunit;

namespace Lemoncode.CryptoFiatConverter.UnitTests.ConverterTests
{
    public class ConvertToEurTests
    {
        [Fact]
        public void Given_Two_Exchange_Updates_When_Converting_Hbar_To_Eur_It_Uses_The_Second_Update_And_Returns_Expected_Values()
        {
            // Given
            var firstUpdatedOn = new DateTime(2021, 10, 1, 0, 0, 0, DateTimeKind.Utc);
            var secondUpdatedOn = firstUpdatedOn.AddMinutes(1);

            var dateTimeFactoryMock = new Mock<IDateTimeFactory>();
            dateTimeFactoryMock
                .SetupSequence(x => x.GetCurrentUtc())
                .Returns(firstUpdatedOn)
                .Returns(secondUpdatedOn);
            var dateTimeFactory = dateTimeFactoryMock.Object;

            var priceDatabase = new InMemoryPriceDatabase(dateTimeFactory);

            priceDatabase.SetAllPrices(
                new Dictionary<string, decimal>
                {
                    { "BTC", 40000.5m },
                    { "ETH", 3000.5m },
                    { "HBAR", 0.30m }
                });

            priceDatabase.SetAllPrices(
                new Dictionary<string, decimal>
                {
                    { "BTC", 28000.0m },
                    { "ETH", 2500.5m },
                    { "HBAR", 0.50m }
                });

            var sut = new Converter(priceDatabase);

            var expectedResult = new ConversionResult("HBAR", secondUpdatedOn, 5);

            // When
            var result = sut.ConvertToEur("HBAR", 10);

            // Then
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
