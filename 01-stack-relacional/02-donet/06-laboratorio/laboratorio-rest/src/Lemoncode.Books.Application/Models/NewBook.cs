using System;
using System.Text.Json.Serialization;

namespace Lemoncode.Books.Application.Models
{
    public class NewBook
    {
        [JsonIgnore]
        public int? Id { get; set; } = default;
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime PublishedOn { get; init; }
        public int AuthorId { get; init; }
    }
}
