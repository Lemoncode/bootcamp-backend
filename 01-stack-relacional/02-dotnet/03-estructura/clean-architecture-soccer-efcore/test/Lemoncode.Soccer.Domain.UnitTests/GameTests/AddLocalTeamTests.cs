using System;
using FluentAssertions;
using Lemoncode.Soccer.Domain.Exceptions;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Domain.UnitTests.GameTests
{
    public static class AddLocalTeamTests
    {
        public class Given_A_Game_When_Adding_A_Local_Team
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _localTeamCode;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _localTeamCode = "RMA";
            }

            protected override void When()
            {
                _sut.AddLocalTeam(_localTeamCode);
            }

            [Fact]
            public void Then_It_Should_Have_The_Expected_Local_Team()
            {
                _sut.LocalTeamCode.Should().Be(_localTeamCode);
            }
            
            [Fact]
            public void Then_It_Should_Have_Null_Foreign_Team_Code()
            {
                _sut.ForeignTeamCode.Should().BeNull();
            }
        }
        
        public class Given_A_Game_When_Adding_A_Local_Team_With_More_Than_Three_Characters
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _localTeamCode;
            private InvalidTeamException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _localTeamCode = "ABCD";
            }

            protected override void When()
            {
                try
                {
                    _sut.AddLocalTeam(_localTeamCode);
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
        
        public class Given_A_Game_When_Adding_A_Local_Team_With_Less_Than_Three_Characters
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _localTeamCode;
            private InvalidTeamException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _localTeamCode = "AB";
            }

            protected override void When()
            {
                try
                {
                    _sut.AddLocalTeam(_localTeamCode);
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
        
        public class Given_A_Game_When_Adding_A_Local_Team_With_Lower_Case_Characters
            : Given_When_Then_Test
        {
            private Game _sut;
            private string _localTeamCode;
            private InvalidTeamException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _localTeamCode = "Rma";
            }

            protected override void When()
            {
                try
                {
                    _sut.AddLocalTeam(_localTeamCode);
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