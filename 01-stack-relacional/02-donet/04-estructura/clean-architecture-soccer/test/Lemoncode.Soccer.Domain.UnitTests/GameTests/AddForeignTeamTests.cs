using System;
using FluentAssertions;
using Lemoncode.Soccer.Domain.Exceptions;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Domain.UnitTests.GameTests
{
    public static class AddForeignTeamTests
    {
        public class Given_A_Game_When_Adding_A_Foreign_Team
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _foreignTeamCode;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _foreignTeamCode = "RMA";
            }

            protected override void When()
            {
                _sut.AddForeignTeam(_foreignTeamCode);
            }

            [Fact]
            public void Then_It_Should_Have_The_Expected_Foreign_Team_Code()
            {
                _sut.ForeignTeamCode.Should().Be(_foreignTeamCode);
            }
            
            [Fact]
            public void Then_It_Should_Have_Null_Local_Team_Code()
            {
                _sut.LocalTeamCode.Should().BeNull();
            }
        }
        
        public class Given_A_Game_When_Adding_A_Foreign_Team_With_More_Than_Three_Characters
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _foreignTeamCode;
            private InvalidTeamException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _foreignTeamCode = "ABCD";
            }

            protected override void When()
            {
                try
                {
                    _sut.AddForeignTeam(_foreignTeamCode);
                }
                catch (InvalidTeamException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_An_InvalidTeamException()
            {
                _exception.Should().NotBeNull();
            }
        }
        
        public class Given_A_Game_When_Adding_A_Foreign_Team_With_Less_Than_Three_Characters
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _foreignTeamCode;
            private InvalidTeamException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _foreignTeamCode = "AB";
            }

            protected override void When()
            {
                try
                {
                    _sut.AddForeignTeam(_foreignTeamCode);
                }
                catch (InvalidTeamException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_An_InvalidTeamException()
            {
                _exception.Should().NotBeNull();
            }
        }
        
        public class Given_A_Game_When_Adding_A_Foreign_Team_With_Lower_Case_Characters
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _foreignTeamCode;
            private InvalidTeamException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _foreignTeamCode = "RMa";
            }

            protected override void When()
            {
                try
                {
                    _sut.AddForeignTeam(_foreignTeamCode);
                }
                catch (InvalidTeamException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_An_InvalidTeamException()
            {
                _exception.Should().NotBeNull();
            }
        }
    }
}