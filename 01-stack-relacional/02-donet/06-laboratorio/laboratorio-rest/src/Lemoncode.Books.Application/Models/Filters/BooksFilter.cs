namespace Lemoncode.Books.Application.Models.Filters
{
    public class BooksFilter
    {
        /// <summary>
        /// The title or partial title for the book
        /// </summary>
        /// <example>foo</example>
        public string? Title { get; init; } = null;

        /// <summary>
        /// The name, last name or partial name for the book's author
        /// </summary>
        /// <example>Joe</example>
        public string? Author { get; init; } = null;
    }
}
