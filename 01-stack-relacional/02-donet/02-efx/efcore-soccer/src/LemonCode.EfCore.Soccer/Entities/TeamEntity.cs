using System.Collections.Generic;

namespace LemonCode.EfCore.Soccer.Entities
{
    public class TeamEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }

        // Navigation properties
        public List<GoalEntity> Goals { get; set; } = new();
    }
}