using System;
using FluentAssertions;
using Lemoncode.Soccer.Domain.Exceptions;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Domain.UnitTests.GameTests
{
    public static class StartTests
    {
        public class Given_A_Game_With_Local_And_Foreign_Teams_When_Starting
            : Given_When_Then_Test
        {
            private Game _sut;
            private DateTime _startedOn;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");

                _startedOn = new DateTime(2021, 02, 01);
            }

            protected override void When()
            {
                _sut.Start(_startedOn);
            }

            [Fact]
            public void Then_It_Should_Be_In_Progress()
            {
                _sut.IsInProgress.Should().BeTrue();
            }
            
            [Fact]
            public void Then_It_Should_Have_The_Expected_StartedOn()
            {
                _sut.StartedOn.Should().Be(_startedOn);
            }
        }
        
        public class Given_A_Game_Without_Teams_When_Starting
            : Given_When_Then_Test
        {
            private Game _sut;
            private DateTime _startedOn;
            private InvalidTeamException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);

                _startedOn = new DateTime(2021, 02, 01);
            }

            protected override void When()
            {
                try
                {
                    _sut.Start(_startedOn);
                }
                catch (InvalidTeamException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_InvalidTeamException()
            {
                _exception.Should().NotBeNull();
            }
            
            [Fact]
            public void Then_It_Should_Not_Be_In_Progress()
            {
                _sut.IsInProgress.Should().BeFalse();
            }
        }
        
        public class Given_A_Game_With_Local_And_Foreign_Teams_And_Already_Started_When_Starting
            : Given_When_Then_Test
        {
            private Game _sut;
            private DateTime _startedOn;
            private GameInProgressException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");
                var previousStartedOn = new DateTime(2021, 02, 01);
                _sut.Start(previousStartedOn);

                _startedOn = previousStartedOn;
            }

            protected override void When()
            {
                try
                {
                    _sut.Start(_startedOn);
                }
                catch (GameInProgressException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_GameInProgressException()
            {
                _exception.Should().NotBeNull();
            }
            
            [Fact]
            public void Then_It_Should_Be_In_Progress()
            {
                _sut.IsInProgress.Should().BeTrue();
            }
            
            [Fact]
            public void Then_It_Should_Not_Be_Ended()
            {
                _sut.IsEnded.Should().BeFalse();
            }
        }
        
        public class Given_A_Game_With_Local_And_Foreign_Teams_And_Already_Finished_When_Starting
            : Given_When_Then_Test
        {
            private Game _sut;
            private DateTime _startedOn;
            private GameEndedException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");
                _sut.Start(new DateTime(2021, 02, 01));
                _sut.End(new DateTime(2021, 02, 01, 2,0,0));
                
                _startedOn = new DateTime(2021, 02, 01);
            }

            protected override void When()
            {
                try
                {
                    _sut.Start(_startedOn);
                }
                catch (GameEndedException exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Throw_GameEndedException()
            {
                _exception.Should().NotBeNull();
            }
            
            [Fact]
            public void Then_It_Should_Not_Be_In_Progress()
            {
                _sut.IsInProgress.Should().BeFalse();
            }
        }
    }
}