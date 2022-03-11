using System.Collections.Generic;
using System.Linq;
using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Application.Models.Filters;
using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lemoncode.Books.Application.Services
{
    public class QueryService
    {
        private readonly BooksDbContext _booksDbContext;

        public QueryService(BooksDbContext booksDbContext)
        {
            _booksDbContext = booksDbContext;
        }

        public AuthorInfo GetAuthor(int id)
        {
            var author = _booksDbContext
                .Authors
                .Include(x => x.Books)
                .SingleOrDefault(x => x.Id == id);
            if (author is null)
            {
                throw new KeyNotFoundException($"Could not find author with Id {id}");
            }

            var authorInfo =
                new AuthorInfo
                {
                    Id = author.Id,
                    FullName = $"{author.Name} {author.LastName}",
                    Birth = author.Birth,
                    CountryCode = author.CountryCode.ToUpperInvariant(),
                    Books = author.Books.Select(x =>
                        new BookSummaryInfo
                        {
                            Id = x.Id,
                            Title = x.Title
                        })
                };

            return authorInfo;
        }

        public BookInfo GetBook(int id)
        {
            var book = _booksDbContext
                .Books
                .Include(x => x.Author)
                .SingleOrDefault(x => x.Id == id);
            if (book is null)
            {
                throw new KeyNotFoundException($"Could not find book with Id {id}");
            }

            var bookInfo = GetAsBookInfo(book);

            return bookInfo;
        }

        public IEnumerable<BookInfo> GetBooks(BooksFilter booksFilter)
        {
            var booksQueryable = _booksDbContext
                .Books
                .Include(x => x.Author)
                .AsQueryable();

            if (booksFilter.Title != null)
            {
                booksQueryable = booksQueryable.Where(x =>
                    x.Title.Contains(booksFilter.Title));
            }

            if (booksFilter.Author != null)
            {
                var author = booksFilter.Author.ToLowerInvariant();
                booksQueryable = booksQueryable.Where(x =>
                    x.Author.Name.Contains(author)
                    || x.Author.LastName.Contains(author));
            }

            var books =
                booksQueryable
                    .ToList()
                    .Select(GetAsBookInfo);

            return books;
        }

        private BookInfo GetAsBookInfo(BookEntity book)
        {
            var bookInfo =
                new BookInfo
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Author = $"{book.Author.Name} {book.Author.LastName}",
                    PublishedOn = book.PublishedOn
                };

            return bookInfo;
        }
    }
}
