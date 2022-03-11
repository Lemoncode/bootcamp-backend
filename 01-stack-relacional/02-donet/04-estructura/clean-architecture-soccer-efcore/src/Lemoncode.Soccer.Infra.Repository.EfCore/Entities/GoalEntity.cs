using System;

namespace Lemoncode.Soccer.Infra.Repository.EfCore.Entities
{
    public class GoalEntity
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int GameId { get; set; }
        public string ScoredBy { get; set; }
        public DateTime ScoredOn { get; set; }

        // Navigation properties
        public TeamEntity Team { get; set; }
        public GameEntity Game { get; set; }
    }
}
