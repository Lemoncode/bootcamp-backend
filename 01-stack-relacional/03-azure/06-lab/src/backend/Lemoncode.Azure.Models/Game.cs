using Lemoncode.Azure.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Lemoncode.Azure.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public int Year { get; set; }
        public string PosterUrl { get; set; }
        public Genre Genre { get; set; }
        public List<Screenshot> Screenshots { get; set; }
        public string DownloadUrl { get; set; }
        public string AgeGroup { get; set; }
        public int Playability { get; set; }
        public int Rating { get; set; }
    }
}
