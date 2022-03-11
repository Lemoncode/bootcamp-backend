using System;

namespace Lemoncode.Books.Application.Models
{
    public class BookInfo
    {
        public int Id { get; set; } = default;
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime PublishedOn { get; init; }
        public string Author { get; init; } = string.Empty;
    }
}
