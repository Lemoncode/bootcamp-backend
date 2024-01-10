using System;
using FluentAssertions;
using Lemoncode.CryptoFiatConverter.Abstractions;
using Xunit;

namespace Lemoncode.CryptoFiatConverter.UnitTests.Models.ConversionResultTests
{
    public class CtorTest
    {
        [Fact]
        public void Given_An_Instance_The_Property_Values_Are_As_Expected()
        {
            // Given
            var cryptoCode = "BTC";
            var rateUpdatedOn = new DateTime(2021, 11, 3, 0, 0, 0);
            var totalEur = 123.456m;

            // When
            var sut = new ConversionResult(cryptoCode, rateUpdatedOn, totalEur);

            // Then
            Assert.Equal(sut.CryptoCode, cryptoCode);
            Assert.Equal(sut.RateUpdatedOn, rateUpdatedOn);
            Assert.Equal(sut.TotalEur, totalEur);
        }

        [Fact]
        public void Given_An_Instance_The_Property_Values_Are_The_Expected()
        {
            // Given
            var cryptoCode = "BTC";
            var rateUpdatedOn = new DateTime(2021, 11, 3, 0, 0, 0);
            var totalEur = 123.456m;

            // When
            var sut = new ConversionResult(cryptoCode, rateUpdatedOn, totalEur);

            // Then
            sut.CryptoCode.Should().Be(cryptoCode);
            sut.RateUpdatedOn.Should().Be(rateUpdatedOn);
            sut.TotalEur.Should().Be(totalEur);
        }
    }
}