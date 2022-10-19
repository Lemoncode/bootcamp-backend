using System;
using System.Collections.Generic;

namespace Lemoncode.Soccer.Infra.Repository.EfCore.Entities
{
    public class GameEntity
    {
        public int Id { get; set; }
        public Guid GameGuid { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime ScheduledOn { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? EndedOn { get; set; }

        // Navigation properties
        public TeamEntity HomeTeam { get; set; }
        public TeamEntity AwayTeam { get; set; }
        public List<GoalEntity> Goals { get; set; } = new();
    }
}
