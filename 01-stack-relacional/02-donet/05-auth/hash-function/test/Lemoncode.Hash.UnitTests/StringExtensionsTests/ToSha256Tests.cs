using FluentAssertions;
using Xunit;

namespace Lemoncode.Hash.UnitTests.StringExtensionsTests
{
    public class ToSha256Tests
    {
        [Fact]
        public void Given_A_Value_Hashed_When_Hashing_Another_Equal_Value_The_Hash_Should_Be_The_Same()
        {
            // Given
            var valueOne = "foo";
            var valueOneHash = valueOne.ToSha256();
            var valueTwo = "foo";

            // When
            var valueTwoHash = valueTwo.ToSha256();

            // Then
            valueTwoHash.Should().BeEquivalentTo(valueOneHash);
        }

        [Fact]
        public void Given_A_Value_Hashed_When_Hashing_Another_Different_Value_The_Hash_Should_Not_Be_The_Same()
        {
            // Given
            var valueOne = "foo";
            var valueOneHash = valueOne.ToSha256();
            var valueTwo = "bar";

            // When
            var valueTwoHash = valueTwo.ToSha256();

            // Then
            valueTwoHash.Should().NotBeEquivalentTo(valueOneHash);
        }
    }
}
