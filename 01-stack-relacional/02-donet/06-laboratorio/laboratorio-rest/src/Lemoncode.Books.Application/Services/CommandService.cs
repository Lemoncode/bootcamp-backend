using System;
using System.Linq;
using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Domain;

namespace Lemoncode.Books.Application.Services
{
    public class CommandService
    {
        private readonly BooksDbContext _booksDbContext;

        public CommandService(BooksDbContext booksDbContext)
        {
            _booksDbContext = booksDbContext;
        }

        public void CreateAuthor(NewAuthor newAuthor)
        {
            var author =
                new AuthorEntity
                {
                    Name = newAuthor.Name,
                    LastName = newAuthor.LastName,
                    Birth = newAuthor.Birth,
                    CountryCode = newAuthor.CountryCode
                };

            _booksDbContext.Authors.Add(author);
            try
            {
                _booksDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed", ex);
            }

            newAuthor.Id = author.Id;
        }

        public void CreateBook(NewBook newBook)
        {
            var book =
                new BookEntity
                {
                    Title = newBook.Title,
                    Description = newBook.Description,
                    PublishedOn = newBook.PublishedOn,
                    AuthorId = newBook.AuthorId
                };
        
            _booksDbContext.Books.Add(book);
            _booksDbContext.SaveChanges();

            newBook.Id = book.Id;
        }

        public void UpdateBook(UpdateBook updateBook)
        {
            var book = _booksDbContext.Books.Single(x => x.Id == updateBook.Id);

            if (!string.IsNullOrWhiteSpace(updateBook.Title))
            {
                book.Title = updateBook.Title;
            }

            if (!string.IsNullOrWhiteSpace(updateBook.Description))
            {
                book.Description = updateBook.Description;
            }

            _booksDbContext.SaveChanges();
        }
    }
}
