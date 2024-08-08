namespace Lemoncode.Soccer.Application.Models
{
    public class NewGame
    {
        /// <summary>
        /// Local Team Code as a 3 uppercase characters
        /// </summary>
        /// <example>RMA</example>
        public string LocalTeamCode { get; set; } = null!;

        /// <summary>
        /// Foreign Team Code as a 3 uppercase characters
        /// </summary>
        /// <example>BAR</example>
        public string ForeignTeamCode { get; set; } = null!;
    }
}