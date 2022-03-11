using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Lemoncode.Books.Application.Models;
using Lemoncode.Books.FunctionalTests.TestSupport;
using Lemoncode.Books.FunctionalTests.TestSupport.Extensions;
using Xunit;

namespace Lemoncode.Books.FunctionalTests.UseCases.GetBooks
{
    public static class GetBooksTests
    {
        public class Given_No_Authentication_When_Getting_Books
            : FunctionalTest
        {
            private HttpResponseMessage _result;
            private string _url;

            protected override Task Given()
            {
                _url = "api/books";
                return Task.CompletedTask;
            }

            protected override async Task When()
            {
                _result = await HttpClient.GetAsync(_url);
            }

            [Fact]
            public void Then_It_Should_Return_Status_Unauthorized()
            {
                _result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        public class Given_An_Author_With_Two_Books_When_Getting_Books
            : FunctionalTest
        {
            private HttpResponseMessage _result;
            private IEnumerable<BookInfo> _expectedBooks;

            protected override async Task Given()
            {
                var newAuthor =
                    new NewAuthor
                    {
                        Name = "foo",
                        LastName = "bar",
                        Birth = 1.ToUtcDate(),
                        CountryCode = "ES"
                    };

                var responseAuthor = await HttpClientAuthorized.PostAsJsonAsync("api/authors", newAuthor);
                if (!responseAuthor.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author");
                }
                var authorIdAsText = responseAuthor.Headers.Location?.AbsolutePath.Split("/").Last();
                var authorId = int.Parse(authorIdAsText!);

                var newBookOne =
                    new NewBook
                    {
                        Title = "title one",
                        Description = "description one",
                        AuthorId = authorId,
                        PublishedOn = 1.ToUtcDate()
                    };

                var responseBookOne = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookOne);
                if (!responseBookOne.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book one");
                }
                var bookOneIdAsText = responseBookOne.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookOneId = int.Parse(bookOneIdAsText!);

                var newBookTwo =
                    new NewBook
                    {
                        Title = "title two",
                        Description = "description two",
                        AuthorId = authorId,
                        PublishedOn = 2.ToUtcDate()
                    };

                var responseBookTwo = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookTwo);
                if (!responseBookTwo.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book two");
                }
                var bookTwoIdAsText = responseBookTwo.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookTwoId = int.Parse(bookTwoIdAsText!);

                _expectedBooks =
                    new List<BookInfo>
                    {
                        new()
                        {
                            Id = bookOneId,
                            Title = newBookOne.Title,
                            Description = newBookOne.Description,
                            PublishedOn = newBookOne.PublishedOn,
                            Author = $"{newAuthor.Name} {newAuthor.LastName}"
                        },
                        new()
                        {
                            Id = bookTwoId,
                            Title = newBookTwo.Title,
                            Description = newBookTwo.Description,
                            PublishedOn = newBookTwo.PublishedOn,
                            Author = $"{newAuthor.Name} {newAuthor.LastName}"
                        }
                    };
            }

            protected override async Task When()
            {
                _result = await HttpClientAuthorized.GetAsync("api/books");
            }

            [Fact]
            public void Then_It_Should_Return_Status_Code_Ok()
            {
                _result.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Fact]
            public async Task Then_It_Should_Return_The_Expected_Author()
            {
                var json = await _result.Content.ReadAsStringAsync();
                var books = JsonSerializer.Deserialize<IEnumerable<BookInfo>>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                books.Should().BeEquivalentTo(_expectedBooks);
            }
        }

        public class Given_An_Author_With_Two_Books_When_Getting_Books_Filtered_By_Title
            : FunctionalTest
        {
            private string _urlWithFilter;
            private HttpResponseMessage _result;
            private IEnumerable<BookInfo> _expectedBooks;

            protected override async Task Given()
            {
                _urlWithFilter = "api/books?title=two";

                var newAuthor =
                    new NewAuthor
                    {
                        Name = "foo",
                        LastName = "bar",
                        Birth = 1.ToUtcDate(),
                        CountryCode = "ES"
                    };

                var responseAuthor = await HttpClientAuthorized.PostAsJsonAsync("api/authors", newAuthor);
                if (!responseAuthor.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author");
                }
                var authorIdAsText = responseAuthor.Headers.Location?.AbsolutePath.Split("/").Last();
                var authorId = int.Parse(authorIdAsText!);

                var newBookOne =
                    new NewBook
                    {
                        Title = "title one",
                        Description = "description one",
                        AuthorId = authorId,
                        PublishedOn = 1.ToUtcDate()
                    };

                var responseBookOne = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookOne);
                if (!responseBookOne.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book one");
                }

                var newBookTwo =
                    new NewBook
                    {
                        Title = "title two",
                        Description = "description two",
                        AuthorId = authorId,
                        PublishedOn = 2.ToUtcDate()
                    };

                var responseBookTwo = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookTwo);
                if (!responseBookTwo.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book two");
                }
                var bookTwoIdAsText = responseBookTwo.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookTwoId = int.Parse(bookTwoIdAsText!);

                _expectedBooks =
                    new List<BookInfo>
                    {
                        new()
                        {
                            Id = bookTwoId,
                            Title = newBookTwo.Title,
                            Description = newBookTwo.Description,
                            PublishedOn = newBookTwo.PublishedOn,
                            Author = $"{newAuthor.Name} {newAuthor.LastName}"
                        }
                    };
            }

            protected override async Task When()
            {
                _result = await HttpClientAuthorized.GetAsync(_urlWithFilter);
            }

            [Fact]
            public void Then_It_Should_Return_Status_Code_Ok()
            {
                _result.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Fact]
            public async Task Then_It_Should_Return_The_Expected_Author()
            {
                var json = await _result.Content.ReadAsStringAsync();
                var books = JsonSerializer.Deserialize<IEnumerable<BookInfo>>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                books.Should().BeEquivalentTo(_expectedBooks);
            }
        }

        public class Given_An_Author_With_One_Book_And_Another_With_Three_Books_When_Getting_Books_Filtered_By_Author
            : FunctionalTest
        {
            private string _urlWithFilter;
            private HttpResponseMessage _result;
            private IEnumerable<BookInfo> _expectedBooks;

            protected override async Task Given()
            {
                _urlWithFilter = "api/books?author=Blog";

                var newAuthorOne =
                    new NewAuthor
                    {
                        Name = "Joe",
                        LastName = "Bloggs",
                        Birth = 1.ToUtcDate(),
                        CountryCode = "ES"
                    };

                var responseAuthorOne = await HttpClientAuthorized.PostAsJsonAsync("api/authors", newAuthorOne);
                if (!responseAuthorOne.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author one");
                }
                var authorOneIdAsText = responseAuthorOne.Headers.Location?.AbsolutePath.Split("/").Last();
                var authorOneId = int.Parse(authorOneIdAsText!);

                var newAuthorTwo =
                    new NewAuthor
                    {
                        Name = "Jane",
                        LastName = "Smith",
                        Birth = 1.ToUtcDate(),
                        CountryCode = "US"
                    };

                var responseAuthorTwo = await HttpClientAuthorized.PostAsJsonAsync("api/authors", newAuthorTwo);
                if (!responseAuthorTwo.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author two");
                }
                var authorTwoIdAsText = responseAuthorTwo.Headers.Location?.AbsolutePath.Split("/").Last();
                var authorTwoId = int.Parse(authorTwoIdAsText!);

                var newBookOne =
                    new NewBook
                    {
                        Title = "title one",
                        Description = "description one",
                        AuthorId = authorOneId,
                        PublishedOn = 1.ToUtcDate()
                    };

                var responseBookOne = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookOne);
                if (!responseBookOne.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book one");
                }
                var bookOneIdAsText = responseBookOne.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookOneId = int.Parse(bookOneIdAsText!);

                var newBookTwo =
                    new NewBook
                    {
                        Title = "title two",
                        Description = "description two",
                        AuthorId = authorOneId,
                        PublishedOn = 2.ToUtcDate()
                    };

                var responseBookTwo = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookTwo);
                if (!responseBookTwo.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book two");
                }
                var bookTwoIdAsText = responseBookTwo.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookTwoId = int.Parse(bookTwoIdAsText!);

                var newBookThree =
                    new NewBook
                    {
                        Title = "title three",
                        Description = "description three",
                        AuthorId = authorTwoId,
                        PublishedOn = 3.ToUtcDate()
                    };

                var responseBookThree = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookThree);
                if (!responseBookThree.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book three");
                }

                _expectedBooks =
                    new List<BookInfo>
                    {
                        new()
                        {
                            Id = bookOneId,
                            Title = newBookOne.Title,
                            Description = newBookOne.Description,
                            PublishedOn = newBookOne.PublishedOn,
                            Author = $"{newAuthorOne.Name} {newAuthorOne.LastName}"
                        },
                        new()
                        {
                            Id = bookTwoId,
                            Title = newBookTwo.Title,
                            Description = newBookTwo.Description,
                            PublishedOn = newBookTwo.PublishedOn,
                            Author = $"{newAuthorOne.Name} {newAuthorOne.LastName}"
                        }
                    };
            }

            protected override async Task When()
            {
                _result = await HttpClientAuthorized.GetAsync(_urlWithFilter);
            }

            [Fact]
            public void Then_It_Should_Return_Status_Code_Ok()
            {
                _result.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Fact]
            public async Task Then_It_Should_Return_The_Expected_Author()
            {
                var json = await _result.Content.ReadAsStringAsync();
                var books = JsonSerializer.Deserialize<IEnumerable<BookInfo>>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                books.Should().BeEquivalentTo(_expectedBooks);
            }
        }

        public class Given_An_Author_With_One_Book_And_Another_With_Three_Books_When_Getting_Books_Filtered_By_Title_And_Author
            : FunctionalTest
        {
            private string _urlWithFilter;
            private HttpResponseMessage _result;
            private IEnumerable<BookInfo> _expectedBooks;

            protected override async Task Given()
            {
                _urlWithFilter = "api/books?title=one&author=Blog";

                var newAuthorOne =
                    new NewAuthor
                    {
                        Name = "Joe",
                        LastName = "Bloggs",
                        Birth = 1.ToUtcDate(),
                        CountryCode = "ES"
                    };

                var responseAuthorOne = await HttpClientAuthorized.PostAsJsonAsync("api/authors", newAuthorOne);
                if (!responseAuthorOne.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author one");
                }
                var authorOneIdAsText = responseAuthorOne.Headers.Location?.AbsolutePath.Split("/").Last();
                var authorOneId = int.Parse(authorOneIdAsText!);

                var newAuthorTwo =
                    new NewAuthor
                    {
                        Name = "Jane",
                        LastName = "Smith",
                        Birth = 1.ToUtcDate(),
                        CountryCode = "US"
                    };

                var responseAuthorTwo = await HttpClientAuthorized.PostAsJsonAsync("api/authors", newAuthorTwo);
                if (!responseAuthorTwo.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author two");
                }
                var authorTwoIdAsText = responseAuthorTwo.Headers.Location?.AbsolutePath.Split("/").Last();
                var authorTwoId = int.Parse(authorTwoIdAsText!);

                var newBookOne =
                    new NewBook
                    {
                        Title = "title one",
                        Description = "description one",
                        AuthorId = authorOneId,
                        PublishedOn = 1.ToUtcDate()
                    };

                var responseBookOne = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookOne);
                if (!responseBookOne.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book one");
                }
                var bookOneIdAsText = responseBookOne.Headers.Location?.AbsolutePath.Split("/").Last();
                var bookOneId = int.Parse(bookOneIdAsText!);

                var newBookTwo =
                    new NewBook
                    {
                        Title = "title two",
                        Description = "description two",
                        AuthorId = authorOneId,
                        PublishedOn = 2.ToUtcDate()
                    };

                var responseBookTwo = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookTwo);
                if (!responseBookTwo.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book two");
                }

                var newBookThree =
                    new NewBook
                    {
                        Title = "title three",
                        Description = "description three",
                        AuthorId = authorTwoId,
                        PublishedOn = 3.ToUtcDate()
                    };

                var responseBookThree = await HttpClientAuthorized.PostAsJsonAsync("api/books", newBookThree);
                if (!responseBookThree.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post book three");
                }

                _expectedBooks =
                    new List<BookInfo>
                    {
                        new()
                        {
                            Id = bookOneId,
                            Title = newBookOne.Title,
                            Description = newBookOne.Description,
                            PublishedOn = newBookOne.PublishedOn,
                            Author = $"{newAuthorOne.Name} {newAuthorOne.LastName}"
                        }
                    };
            }

            protected override async Task When()
            {
                _result = await HttpClientAuthorized.GetAsync(_urlWithFilter);
            }

            [Fact]
            public void Then_It_Should_Return_Status_Code_Ok()
            {
                _result.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Fact]
            public async Task Then_It_Should_Return_The_Expected_Author()
            {
                var json = await _result.Content.ReadAsStringAsync();
                var books = JsonSerializer.Deserialize<IEnumerable<BookInfo>>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                books.Should().BeEquivalentTo(_expectedBooks);
            }
        }
    }
}