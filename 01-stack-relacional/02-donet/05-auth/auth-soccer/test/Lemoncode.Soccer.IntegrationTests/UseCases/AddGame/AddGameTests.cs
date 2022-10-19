using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Lemoncode.Soccer.IntegrationTests.TestSupport;
using Lemoncode.Soccer.IntegrationTests.TestSupport.Builders;
using Xunit;

namespace Lemoncode.Soccer.IntegrationTests.UseCases.AddGame
{
    public static class AddGameTests
    {
        public class Given_A_NewGame_When_Posting
            : FunctionalTest
        {
            private string _url;
            private HttpResponseMessage _result;
            private StringContent _content;

            protected override Task Given()
            {
                _url = "games";
                var jsonStream =
                    new FileStreamBuilder()
                        .WithFileResourceName("add_game_new_game.json")
                        .Build();
                _content =
                    new StringContentBuilder()
                        .WithContent(jsonStream)
                        .Build();
                
                return Task.CompletedTask;
            }

            protected override async Task When()
            {
                _result = await HttpClient.PostAsync(_url, _content);
            }
            
            [Fact]
            public void Then_It_Should_Return_201_Created()
            {
                _result.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            [Fact]
            public void Then_It_Should_Return_A_Location_Header()
            {
                _result.Headers.Location.Should().NotBeNull();
            }
        }
    }
}