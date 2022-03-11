using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonCode.EfCore.Soccer.Entities
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
