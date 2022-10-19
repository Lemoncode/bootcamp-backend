using System;
using FluentAssertions;
using Lemoncode.Soccer.Domain.Exceptions;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Domain.UnitTests.GameTests
{
    public static class EndTests
    {
        public class Given_A_Game_Started_When_Ending
            : Given_When_Then_Test
        {
            private Game _sut;
            private DateTime _endedOn;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");
                var startedOn = new DateTime(2021, 02, 01);
                _sut.Start(startedOn);

                _endedOn = startedOn.AddHours(2);
            }

            protected override void When()
            {
                _sut.End(_endedOn);
            }

            [Fact]
            public void Then_It_Should_Be_Ended()
            {
                _sut.IsEnded.Should().BeTrue();
            }
            
            [Fact]
            public void Then_It_Should_Not_Be_In_Progress()
            {
                _sut.IsInProgress.Should().BeFalse();
            }
            
            [Fact]
            public void Then_It_Should_Have_The_Expected_EndedOn()
            {
                _sut.EndedOn.Should().Be(_endedOn);
            }
        }
        
        public class Given_A_Game_Not_Started_When_Ending
            : Given_When_Then_Test
        {
            private Game _sut;
            private DateTime _endedOn;
            private GameNotInProgressException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");

                _endedOn = new DateTime(2021, 02, 01);
            }

            protected override void When()
            {
                try
                {
                    _sut.End(_endedOn);
                }
                catch (GameNotInProgressException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_A_GameNotInProgressException()
            {
                _exception.Should().NotBeNull();
            }
            
            [Fact]
            public void Then_It_Should_Not_Be_In_Progress()
            {
                _sut.IsInProgress.Should().BeFalse();
            }
            
            [Fact]
            public void Then_It_Should_Not_Be_Ended()
            {
                _sut.IsEnded.Should().BeFalse();
            }
        }
        
        public class Given_A_Game_Started_And_An_EndsOn_Prior_To_StartedOn_When_Ending
            : Given_When_Then_Test
        {
            private Game _sut;
            private DateTime _endedOn;
            private IncorrectDateTimeException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");
                
                var startedOn = new DateTime(2021, 02, 01);
                _sut.Start(startedOn);

                _endedOn = startedOn.AddSeconds(-1);
            }

            protected override void When()
            {
                try
                {
                    _sut.End(_endedOn);
                }
                catch (IncorrectDateTimeException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_An_IncorrectDateTimeException()
            {
                _exception.Should().NotBeNull();
            }
            
            [Fact]
            public void Then_It_Should_Not_Be_Ended()
            {
                _sut.IsEnded.Should().BeFalse();
            }
        }
    }
}