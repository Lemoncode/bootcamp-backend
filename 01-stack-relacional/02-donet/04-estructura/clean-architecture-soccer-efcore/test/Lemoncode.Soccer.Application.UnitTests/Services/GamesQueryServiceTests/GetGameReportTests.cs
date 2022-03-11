using System;
using FluentAssertions;
using Lemoncode.Soccer.Application.Mappers;
using Lemoncode.Soccer.Application.Services;
using Lemoncode.Soccer.Domain;
using Lemoncode.TestSupport;
using Moq;
using Xunit;

namespace Lemoncode.Soccer.Application.UnitTests.Services.GamesQueryServiceTests
{
    public static class GetGameReportTests
    {
        public class Given_A_Game_Id_When_Getting_Game_Report
            : Given_When_Then_Test
        {
            private GamesQueryService _sut;
            private Mock<IGamesRepository> _gamesRepositoryMock;
            private Guid _id;
            private Exception _exception;

            protected override void Given()
            {
                _id = Guid.Empty;
                
                var game = new Game(_id);
                game.AddLocalTeam("RMA");
                game.AddForeignTeam("BAR");
                
                _gamesRepositoryMock = new Mock<IGamesRepository>();
                _gamesRepositoryMock
                    .Setup(x => x.GetGame(_id))
                    .Returns(game);
                var gamesRepository = _gamesRepositoryMock.Object;
                
                var gameToGameReportMapper = new GameToGameReportMapper();
                _sut = new GamesQueryService(gamesRepository, gameToGameReportMapper);
            }

            protected override void When()
            {
                try
                {
                    _ = _sut.GetGameReport(_id);
                }
                catch (Exception exception)
                {
                    _exception = exception;
                }
            }

            [Fact]
            public void Then_It_Should_Use_The_GamesRepository_With_The_Expected_Id()
            {
                _gamesRepositoryMock.Verify(x => x.GetGame(_id));
            }
            
            [Fact]
            public void Then_It_Should_Not_Throw_Any_Exception()
            {
                _exception.Should().BeNull();
            }
        }
    }
}