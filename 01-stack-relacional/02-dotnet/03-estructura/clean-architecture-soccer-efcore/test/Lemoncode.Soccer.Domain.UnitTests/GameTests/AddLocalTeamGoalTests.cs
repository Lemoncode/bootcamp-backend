using System;
using System.Linq;
using FluentAssertions;
using Lemoncode.Soccer.Domain.Exceptions;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Domain.UnitTests.GameTests
{
    public static class AddLocalTeamGoalTests
    {
        public class Given_A_Game_In_Progress_When_Adding_Local_Team_Goal
            : Given_When_Then_Test
        {
            private Game _sut;
            private Goal _goal;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");

                var startedOn = new DateTime(2021, 02, 01);
                _sut.Start(startedOn);

                var scoredOn = startedOn.AddMinutes(10);
                _goal = new Goal(scoredOn, "Benzema");
            }

            protected override void When()
            {
                _sut.AddLocalTeamGoal(_goal);
            }

            [Fact]
            public void Then_It_Should_Have_Score_One_For_Local_Team()
            {
                _sut.LocalGoals.Should().HaveCount(1);
            }
            
            [Fact]
            public void Then_It_Should_Have_The_Expected_Score()
            {
                _sut.LocalGoals.Single().Should().BeEquivalentTo(_goal);
            }
            
            [Fact]
            public void Then_It_Should_Have_Score_Zero_For_Foreign_Team()
            {
                _sut.ForeignGoals.Should().HaveCount(0);
            }
        }
        
        public class Given_A_Game_In_Progress_And_A_Local_Goal_When_Adding_Local_Team_Goal
            : Given_When_Then_Test
        {
            private Game _sut;
            private Goal _goal;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");

                var startedOn = new DateTime(2021, 02, 01);
                _sut.Start(startedOn);

                var firstScoredOn = startedOn.AddMinutes(10);
                var firstGoal = new Goal(firstScoredOn, "Benzema");
                _sut.AddLocalTeamGoal(firstGoal);

                var scoredOn = startedOn.AddMinutes(15);
                _goal = new Goal(scoredOn, "Ramos");
            }

            protected override void When()
            {
                _sut.AddLocalTeamGoal(_goal);
            }

            [Fact]
            public void Then_It_Should_Have_Score_Two_For_Local_Team()
            {
                _sut.LocalGoals.Should().HaveCount(2);
            }
            
            [Fact]
            public void Then_It_Should_Have_The_Expected_Last_Score()
            {
                _sut.LocalGoals.Last().Should().BeEquivalentTo(_goal);
            }
            
            [Fact]
            public void Then_It_Should_Have_Score_Zero_For_Foreign_Team()
            {
                _sut.ForeignGoals.Should().HaveCount(0);
            }
        }
        
        public class Given_A_Game_Not_Started_When_Adding_Local_Team_Goal
            : Given_When_Then_Test
        {
            private Game _sut;
            private Goal _goal;
            private GameNotInProgressException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");

                var scoredOn = new DateTime(2021, 02, 01);
                _goal = new Goal(scoredOn, "Benzema");
            }

            protected override void When()
            {
                try
                {
                    _sut.AddLocalTeamGoal(_goal);
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
        }
        
        public class Given_A_Game_Already_Ended_When_Adding_Local_Team_Goal
            : Given_When_Then_Test
        {
            private Game _sut;
            private Goal _goal;
            private GameNotInProgressException _exception;

            protected override void Given()
            {
                var id = Guid.Empty;
                _sut = new Game(id);
                
                _sut.AddLocalTeam("RMA");
                _sut.AddForeignTeam("BAR");
                
                var startedOn = new DateTime(2021, 02, 01);
                _sut.Start(startedOn);

                var endedOn = startedOn.AddHours(2);
                _sut.End(endedOn);

                var scoredOn = startedOn.AddMinutes(10);
                _goal = new Goal(scoredOn, "Benzema");
            }

            protected override void When()
            {
                try
                {
                    _sut.AddLocalTeamGoal(_goal);
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
        }
    }
}