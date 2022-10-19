using System;
using System.Collections.Generic;

namespace Lemoncode.Soccer.Application.Models
{
    public class GameReportDetailed
        : GameReport
    {
        public IEnumerable<string> LocalGoalsReport { get; }
        public IEnumerable<string> ForeignGoalsReport { get; }
        public new string Result => $"{LocalTeamName} {LocalGoals} - {ForeignGoals} {ForeignTeamName}";

        public GameReportDetailed(Guid id, string localTeamName, int localGoals, string foreignTeamName, int foreignGoals, IEnumerable<string> localGoalsReport, IEnumerable<string> foreignGoalsReport)
            : base(id, localTeamName, localGoals, foreignTeamName, foreignGoals)
        {
            LocalGoalsReport = localGoalsReport;
            ForeignGoalsReport = foreignGoalsReport;
        }
    }
}