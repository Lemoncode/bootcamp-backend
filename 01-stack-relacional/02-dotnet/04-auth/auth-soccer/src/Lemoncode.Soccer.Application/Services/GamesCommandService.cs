using System;
using Lemoncode.Soccer.Application.Exceptions;
using Lemoncode.Soccer.Application.Models;
using Lemoncode.Soccer.Domain;

namespace Lemoncode.Soccer.Application.Services
{
    public class GamesCommandService
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IDateTimeService _dateTimeService;

        public GamesCommandService(
            IGamesRepository gamesRepository, 
            IDateTimeService dateTimeService)
        {
            _gamesRepository = gamesRepository;
            _dateTimeService = dateTimeService;
        }
        
        public Guid CreateGame(NewGame newGame)
        {
            var newId = Guid.NewGuid();
            var game = new Game(newId);
            
            game.AddLocalTeam(newGame.LocalTeamCode);
            game.AddForeignTeam(newGame.ForeignTeamCode);
            
            _gamesRepository.AddGame(game);
            
            return newId;
        }

        public void SetProgress(Guid id, GameProgress gameProgress)
        {
            var game = _gamesRepository.GetGame(id);
            var currentDate = _dateTimeService.GetUtcNow();
            if (gameProgress.IsInProgress)
            {
                game.Start(currentDate);
            }
            else
            {
                game.End(currentDate);
            }
            
            _gamesRepository.UpdateGame(id, game);
        }

        public void AddGoal(Guid id, NewGoal newGoal)
        {
            var game = _gamesRepository.GetGame(id);
            var currentDate = _dateTimeService.GetUtcNow();
            var teamCode = newGoal.TeamCode;
            var goal = new Goal(currentDate, newGoal.ScoredBy);
            var isTeamPlaying = game.LocalTeamCode == teamCode || game.ForeignTeamCode == teamCode;
            if (!isTeamPlaying)
            {
                throw new ResourceNotFoundException($"The team code {teamCode} is not playing the game");
            }
            
            if (game.LocalTeamCode == teamCode)
            {
                game.AddLocalTeamGoal(goal);
            }
            else
            {
                game.AddForeignTeamGoal(goal);
            }

            _gamesRepository.UpdateGame(id, game);
        }
    }
}