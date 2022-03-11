using System;
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

namespace Lemoncode.Books.FunctionalTests.UseCases.GetAuthor
{
    public static class GetBooksTests
    {
        public class Given_An_Author_When_Getting_Author
            : FunctionalTest
        {
            private NewAuthor _newAuthor;
            private HttpResponseMessage _result;
            private string _authorId;
            private AuthorInfo _expectedAuthorInfo;

            protected override async Task Given()
            {
                _newAuthor =
                    new NewAuthor
                    {
                        Name = "foo",
                        LastName = "bar",
                        Birth = 1.ToUtcDate(),
                        CountryCode = "ES"
                    };

                var response = await HttpClientAuthorized.PostAsJsonAsync("api/authors", _newAuthor);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Failed to post author");
                }
                _authorId = response.Headers.Location?.AbsolutePath.Split("/").Last();

                _expectedAuthorInfo =
                    new AuthorInfo
                    {
                        Id = int.Parse(_authorId!),
                        FullName = $"{_newAuthor.Name} {_newAuthor.LastName}",
                        Birth = _newAuthor.Birth,
                        CountryCode = _newAuthor.CountryCode,
                        Books = Enumerable.Empty<BookSummaryInfo>()
                    };
            }

            protected override async Task When()
            {
                _result = await HttpClientAuthorized.GetAsync($"api/authors/{_authorId}");
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
                var authorInfo = JsonSerializer.Deserialize<AuthorInfo>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                authorInfo.Should().BeEquivalentTo(_expectedAuthorInfo);
            }
        }
    }
}