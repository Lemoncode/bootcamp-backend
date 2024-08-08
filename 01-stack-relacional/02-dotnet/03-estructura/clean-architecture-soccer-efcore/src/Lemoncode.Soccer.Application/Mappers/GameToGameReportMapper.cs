using System;
using System.Linq;
using Lemoncode.Soccer.Application.Models;
using Lemoncode.Soccer.Domain;

namespace Lemoncode.Soccer.Application.Mappers
{
    public class GameToGameReportMapper
    {
        public GameReport Map(Game game, bool isDetailed = false)
        {
            var gameId = game.Id;
            var localTeamCode = game.LocalTeamCode;
            var localTeamGoalsCount = game.LocalGoals.Count;
            var foreignTeamCode = game.ForeignTeamCode;
            var foreignTeamGoalsCount = game.ForeignGoals.Count;
            if (isDetailed)
            {
                var startedOn = game.StartedOn;
                var localTeamGoalsReport =
                    game.LocalGoals.Select(x => GetFormattedScorer(startedOn!.Value, x));
                var foreignTeamGoalsReport =
                    game.ForeignGoals.Select(x => GetFormattedScorer(startedOn!.Value, x));
                return new GameReportDetailed(gameId, localTeamCode, localTeamGoalsCount, foreignTeamCode, foreignTeamGoalsCount, localTeamGoalsReport, foreignTeamGoalsReport);
            }
            
            return new GameReport(gameId, localTeamCode, localTeamGoalsCount, foreignTeamCode, foreignTeamGoalsCount);
        }

        private string GetFormattedScorer(DateTime startedOn, Goal goal)
        {
            var minute = GetGameMinute(startedOn, goal.ScoredOn);
            return $"{minute} {goal.ScoredBy}";
        }
        private string GetGameMinute(DateTime startedOn, DateTime scoredOn)
        {
            var timeSpan = scoredOn.Subtract(startedOn);
            return $"{(int) timeSpan.TotalMinutes}'";
        }
    }
}