using System;
using System.Collections.Generic;
using System.Linq;
using Lemoncode.Soccer.Application;
using Lemoncode.Soccer.Domain;
using Lemoncode.Soccer.Infra.Repository.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lemoncode.Soccer.Infra.Repository.EfCore
{
    public class EntityFrameworkRepository
        : IGamesRepository
    {
        private readonly SoccerContext _soccerContext;

        public EntityFrameworkRepository(SoccerContext soccerContext)
        {
            _soccerContext = soccerContext;
        }

        public IEnumerable<Game> GetGames()
        {
            var gameEntities =
                _soccerContext
                    .Games
                    .Include(x => x.AwayTeam)
                    .Include(x => x.HomeTeam)
                    .Include(x => x.Goals)
                    .ToList();
            var games = gameEntities.Select(MapGameEntityToGame);
            return games;
        }

        public Game GetGame(Guid id)
        {
            var gameEntity =
                _soccerContext
                    .Games
                    .Include(x => x.HomeTeam)
                    .Include(x => x.AwayTeam)
                    .Include(x => x.Goals)
                    .SingleOrDefault(x => x.GameGuid == id);
            if (gameEntity is null)
            {
                throw new KeyNotFoundException($"Could not find game with Id {id}");
            }

            var game = MapGameEntityToGame(gameEntity);
            return game;
        }

        public void AddGame(Game game)
        {
            var gameEntity = MapGameToGameEntity(game);
            _soccerContext.Games.Add(gameEntity);
            _soccerContext.SaveChanges();
        }

        public void RemoveGame(Guid id)
        {
            var gameEntity = _soccerContext.Games.SingleOrDefault(x => x.GameGuid == id);
            if (gameEntity != null)
            {
                _soccerContext.Games.Remove(gameEntity);
                _soccerContext.SaveChanges();
            }
        }

        public void UpdateGame(Guid id, Game game)
        {
            var gameEntity = _soccerContext.Games.SingleOrDefault(x => x.GameGuid == id);
            if (gameEntity is null)
            {
                throw new KeyNotFoundException($"Could not update non existing game with Guid {id}");
            }
            var updatedGame = MapGameToGameEntity(game);
            gameEntity.Goals = updatedGame.Goals;
            gameEntity.AwayTeamId = updatedGame.AwayTeamId;
            gameEntity.HomeTeamId = updatedGame.HomeTeamId;
            gameEntity.StartedOn = updatedGame.StartedOn;
            gameEntity.EndedOn = updatedGame.EndedOn;
            _soccerContext.SaveChanges();
        }

        private TeamEntity GetTeamByCode(string code)
        {
            var teamEntity = _soccerContext.Teams.SingleOrDefault(x => x.Code == code);
            return teamEntity;
        }

        private Game MapGameEntityToGame(GameEntity gameEntity)
        {
            var game = new Game(gameEntity.GameGuid);

            // Use reflection to update private setters. This is the downside of using this RDBMS approach if we don't want to break encapsulation in our domain
            var localTeamCodeSetter = game.GetType().GetProperty(nameof(Game.LocalTeamCode));
            localTeamCodeSetter?.SetValue(game, gameEntity.HomeTeam.Code);

            var foreignTeamCodeSetter = game.GetType().GetProperty(nameof(Game.ForeignTeamCode));
            foreignTeamCodeSetter?.SetValue(game, gameEntity.AwayTeam.Code);

            var startedOnSetter = game.GetType().GetProperty(nameof(Game.StartedOn));
            startedOnSetter?.SetValue(game, gameEntity.StartedOn);

            var endedOnSetter = game.GetType().GetProperty(nameof(Game.EndedOn));
            endedOnSetter?.SetValue(game, gameEntity.EndedOn);

            var localGoalEntities = gameEntity.Goals.Where(x => x.TeamId == gameEntity.HomeTeamId).ToList();
            var localGoals = localGoalEntities.Select(MapGoalEntityToGoal);
            foreach (var localGoal in localGoals)
            {
                game.AddLocalTeamGoal(localGoal);
            }

            var foreignGoalEntities = gameEntity.Goals.Where(x => x.TeamId == gameEntity.AwayTeamId).ToList();
            var foreignGoals = foreignGoalEntities.Select(MapGoalEntityToGoal);
            foreach (var foreignGoal in foreignGoals)
            {
                game.AddForeignTeamGoal(foreignGoal);
            }

            return game;
        }

        private GameEntity MapGameToGameEntity(Game game)
        {
            var localGoals = game.LocalGoals;
            var foreignGoals = game.ForeignGoals;

            var localTeamCode = game.LocalTeamCode;
            var localTeamEntity = GetTeamByCode(game.LocalTeamCode);
            if (localTeamEntity is null)
            {
                var newTeamEntity = new TeamEntity { Code = localTeamCode };
                _soccerContext.Teams.Add(newTeamEntity);
                _soccerContext.SaveChanges();
                localTeamEntity = newTeamEntity;
            }

            var foreignTeamCode = game.ForeignTeamCode;
            var foreignTeamEntity = GetTeamByCode(foreignTeamCode);
            if (foreignTeamEntity is null)
            {
                var newTeamEntity = new TeamEntity { Code = foreignTeamCode };
                _soccerContext.Teams.Add(newTeamEntity);
                _soccerContext.SaveChanges();
                foreignTeamEntity = newTeamEntity;
            }

            var localGoalEntities = localGoals.Select(x => MapGoalToGoalEntity(x, localTeamEntity.Id)).ToList();
            var foreignGoalEntities = foreignGoals.Select(x => MapGoalToGoalEntity(x, foreignTeamEntity.Id)).ToList();

            var allGoalEntities = localGoalEntities.Concat(foreignGoalEntities).ToList();

            var gameEntity =
                new GameEntity
                {
                    GameGuid = game.Id,
                    AwayTeamId = foreignTeamEntity.Id,
                    HomeTeamId = localTeamEntity.Id,
                    Goals = allGoalEntities,
                    StartedOn = game.StartedOn,
                    EndedOn = game.EndedOn
                };

            return gameEntity;
        }

        private GoalEntity MapGoalToGoalEntity(Goal goal, int teamId)
        {
            var goalEntity =
                new GoalEntity
                {
                    ScoredBy = goal.ScoredBy,
                    ScoredOn = goal.ScoredOn,
                    TeamId = teamId
                };

            return goalEntity;
        }

        private Goal MapGoalEntityToGoal(GoalEntity goalEntity)
        {
            var goal = new Goal(goalEntity.ScoredOn, goalEntity.ScoredBy);
            return goal;
        }
    }

}
