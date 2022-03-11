using System;

namespace Lemoncode.Soccer.Application.Models
{
    public class GameReport
    {
        public Guid Id { get; }
        public string LocalTeamName { get; }
        public int LocalGoals { get; }
        public string ForeignTeamName { get; }
        public int ForeignGoals { get; }
        public string Result => $"{LocalTeamName} {LocalGoals} - {ForeignGoals} {ForeignTeamName}";

        public GameReport(Guid id, string localTeamName, int localGoals, string foreignTeamName, int foreignGoals)
        {
            Id = id;
            LocalTeamName = localTeamName;
            LocalGoals = localGoals;
            ForeignTeamName = foreignTeamName;
            ForeignGoals = foreignGoals;
        }
    }
}