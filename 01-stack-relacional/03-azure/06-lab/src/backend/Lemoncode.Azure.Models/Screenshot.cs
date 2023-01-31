using System.ComponentModel.DataAnnotations;

namespace Lemoncode.Azure.Models
{
    public class Screenshot
    {
        [Key]
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
