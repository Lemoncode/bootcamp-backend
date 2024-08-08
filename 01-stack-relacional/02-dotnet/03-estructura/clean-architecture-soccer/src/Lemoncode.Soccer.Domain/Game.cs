using System;
using System.Collections.Generic;
using System.Linq;
using Lemoncode.Soccer.Domain.Exceptions;

namespace Lemoncode.Soccer.Domain
{
    public class Game
    {
        public Guid Id { get; }
        public string LocalTeamCode { get; private set; }
        public string ForeignTeamCode { get; private set; }
        public DateTime? StartedOn { get; private set; }
        public bool IsInProgress => StartedOn.HasValue && !EndedOn.HasValue;
        public DateTime? EndedOn { get; private set; }
        public bool IsEnded => EndedOn.HasValue && StartedOn.HasValue;
        
        private readonly List<Goal> _localGoals = new List<Goal>();
        public IReadOnlyCollection<Goal> LocalGoals => _localGoals.AsReadOnly();
        
        private readonly List<Goal> _foreignGoals = new List<Goal>();
        public IReadOnlyCollection<Goal> ForeignGoals => _foreignGoals.AsReadOnly();
        
        public Game(Guid id)
        {
            Id = id;
            LocalTeamCode = null!;
            ForeignTeamCode = null!;
        }

        public void AddLocalTeam(string localTeamCode)
        {
            if (!IsValidTeam(localTeamCode))
            {
                throw new InvalidTeamException($"Invalid team code {localTeamCode}. It must have 3 upper case characters");
            }
            LocalTeamCode = localTeamCode;
        }
        
        public void AddForeignTeam(string foreignTeamCode)
        {
            if (!IsValidTeam(foreignTeamCode))
            {
                throw new InvalidTeamException($"Invalid team code {foreignTeamCode}. It must have 3 upper case characters");
            }
            ForeignTeamCode = foreignTeamCode;
        }
        
        public void Start(DateTime startedOn)
        {
            if (LocalTeamCode == null || ForeignTeamCode == null)
            {
                throw new InvalidTeamException($"Unknown teams in the game, local: {LocalTeamCode}, foreign: {ForeignTeamCode}");
            }

            if (IsInProgress)
            {
                throw new GameInProgressException();
            }
            
            if (IsEnded)
            {
                throw new GameEndedException();
            }
            
            StartedOn = startedOn;
        }
        
        public void End(DateTime endedOn)
        {
            if (!IsInProgress)
            {
                throw new GameNotInProgressException();
            }
            
            if (endedOn <= StartedOn)
            {
                throw new IncorrectDateTimeException($"The game started on {StartedOn} and cannot end prior to that time");
            }
            
            EndedOn = endedOn;
        }

        public void AddLocalTeamGoal(Goal goal)
        {
            if (!IsInProgress)
            {
                throw new GameNotInProgressException();
            }
            
            _localGoals.Add(goal);
        }
        
        public void AddForeignTeamGoal(Goal goal)
        {
            if (!IsInProgress)
            {
                throw new GameNotInProgressException();
            }
            
            _foreignGoals.Add(goal);
        }

        private bool IsValidTeam(string teamCode)
        {
            return teamCode.Length == 3 && !teamCode.Any(char.IsLower);
        }
    }
}