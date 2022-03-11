using System;
using FluentAssertions;
using Lemoncode.Soccer.Application.Mappers;
using Lemoncode.Soccer.Application.Models;
using Lemoncode.Soccer.Domain;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Application.UnitTests.Mappers.GameToGameReportMapperTests
{
    public static class MapTests
    {
        public class Given_A_Game_When_Mapping_To_A_GameReport
            : Given_When_Then_Test
        {
            private GameToGameReportMapper _sut;
            private Game _game;
            private GameReport _result;
            private GameReport _expectedGameReport;

            protected override void Given()
            {
                _sut = new GameToGameReportMapper();

                var id = Guid.Empty;
                _game = new Game(id);
                _expectedGameReport = new GameReport(id, null!, 0, null!, 0);
            }

            protected override void When()
            {
                _result = _sut.Map(_game);
            }

            [Fact]
            public void Then_It_Should_Be_Equivalent_To_The_Expected_GameReport()
            {
                _result.Should().BeEquivalentTo(_expectedGameReport);
            }
        }
    }
}