using System;

namespace Lemoncode.Books.Domain
{
    public class BookEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedOn { get; set; }
        public int AuthorId { get; set; }
        public AuthorEntity Author { get; set; }
    }
}
