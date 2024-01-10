using System;
using FluentAssertions;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Domain.UnitTests.GameTests
{
    public static class ConstructorTest
    {
        public class Given_Valid_Dependencies_When_Constructing_Instance
            : Given_When_Then_Test
        {
            private Game _sut;
            private Guid _id;

            protected override void Given()
            {
                _id = Guid.Empty;
            }

            protected override void When()
            {
                _sut = new Game(_id);
            }

            [Fact]
            public void Then_It_Should_Have_Created_A_Valid_Instance()
            {
                _sut.Should().NotBeNull();
            }
            
            [Fact]
            public void Then_It_Should_Have_The_Expected_Id()
            {
                _sut.Id.Should().Be(_id);
            }
            
            [Fact]
            public void Then_It_Should_Have_Null_LocalTeam()
            {
                _sut.LocalTeamCode.Should().BeNull();
            }
            
            [Fact]
            public void Then_It_Should_Have_Null_ForeignTeam()
            {
                _sut.ForeignTeamCode.Should().BeNull();
            }
        }
    }
}