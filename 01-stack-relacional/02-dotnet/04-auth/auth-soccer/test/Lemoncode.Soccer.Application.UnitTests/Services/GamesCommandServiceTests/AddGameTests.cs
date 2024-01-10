using System;
using FluentAssertions;
using Lemoncode.Soccer.Application.Models;
using Lemoncode.Soccer.Application.Services;
using Lemoncode.Soccer.Domain;
using Lemoncode.TestSupport;
using Moq;
using Xunit;

namespace Lemoncode.Soccer.Application.UnitTests.Services.GamesCommandServiceTests
{
    public static class AddGameTests
    {
        public class Given_A_NewGame_When_Creating
            : Given_When_Then_Test
        {
            private GamesCommandService _sut;
            private Mock<IGamesRepository> _gamesRepositoryMock;
            private NewGame _newGame;
            private Exception _exception;
            private Guid _result;
            private string _localTeamCode;
            private string _foreignTeamCode;

            protected override void Given()
            {
                _gamesRepositoryMock = new Mock<IGamesRepository>();
                var gamesRepository = _gamesRepositoryMock.Object;
                var dateTimeService = Mock.Of<IDateTimeService>();
                _sut = new GamesCommandService(gamesRepository, dateTimeService);

                _localTeamCode = "RMA";
                _foreignTeamCode = "BAR";
                _newGame =
                    new NewGame
                    {
                        LocalTeamCode = _localTeamCode,
                        ForeignTeamCode = _foreignTeamCode
                    };
            }

            protected override void When()
            {
                try
                {
                    _result = _sut.CreateGame(_newGame);
                }
                catch (Exception exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Use_The_GamesRepository_To_Save_A_Game()
            {
                _gamesRepositoryMock.Verify(x => 
                    x.AddGame(It.Is<Game>(game => game.LocalTeamCode == _localTeamCode && game.ForeignTeamCode == _foreignTeamCode)));
            }
            
            [Fact]
            public void Then_It_Should_Return_A_Valid_Id()
            {
                _result.Should().NotBeEmpty();
            }
        }
    }
}