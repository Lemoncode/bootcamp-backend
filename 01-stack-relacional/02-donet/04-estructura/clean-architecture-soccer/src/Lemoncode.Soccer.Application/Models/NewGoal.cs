namespace Lemoncode.Soccer.Application.Models
{
    public class NewGoal
    {
        /// <summary>
        /// The name of the scorer
        /// </summary>
        /// <example>Benzema</example>
        public string ScoredBy { get; set; } = null!;

        /// <summary>
        /// The team code for the team adding the goal to its score
        /// </summary>
        /// <example>RMA</example>
        public string TeamCode { get; set; } = null!;
    }
}