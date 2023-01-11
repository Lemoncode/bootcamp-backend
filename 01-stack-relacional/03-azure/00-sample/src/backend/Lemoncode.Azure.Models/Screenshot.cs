using System.ComponentModel.DataAnnotations;

namespace Lemoncode.Azure.Api.Models
{
    public class Screenshot
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
