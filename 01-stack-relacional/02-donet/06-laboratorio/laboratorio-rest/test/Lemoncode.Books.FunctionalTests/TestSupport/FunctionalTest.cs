using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lemoncode.Books.Application;
using Lemoncode.Books.FunctionalTests.TestSupport.Extensions;
using Lemoncode.Books.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lemoncode.Books.FunctionalTests.TestSupport
{
    public abstract class FunctionalTest
        : Given_When_Then_Test_Async
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _uniqueDatabaseName;
        protected HttpClient HttpClient { get; }
        protected HttpClient HttpClientAuthorized { get; }
        protected IConfiguration Configuration { get; }
        protected FunctionalTest()
        {
            var server =
                new TestServer(
                    new WebHostBuilder()
                        .UseStartup<Startup>()
                        .UseCommonConfiguration()
                        .UseEnvironment("Test")
                        .ConfigureTestServices(ConfigureTestServices));

            _serviceProvider = server.Services;
            Configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            // Http clients
            HttpClient = server.CreateClient();
            
            var authorizationHeaderValue = GetBasicAuthorizationHeaderValue(Configuration);
            var httpClientAuthorized = server.CreateClient();
            httpClientAuthorized.DefaultRequestHeaders.Authorization = authorizationHeaderValue;
            HttpClientAuthorized = httpClientAuthorized;
            
            // Apply Migrations
            _uniqueDatabaseName = $"Test-{Guid.NewGuid()}";
            using var dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<BooksDbContext>();
            dbContext.Database.Migrate();
        }

        private AuthenticationHeaderValue GetBasicAuthorizationHeaderValue(IConfiguration configuration)
        {
            var testUsername = configuration.GetValue<string>("BasicAuthentication:Username");
            var testPassword = configuration.GetValue<string>("BasicAuthentication:Password");
            var credentials = $"{testUsername}:{testPassword}";
            var credentialsBytes = Encoding.UTF8.GetBytes(credentials);
            var credentialsBase64 = Convert.ToBase64String(credentialsBytes);
            var result = new AuthenticationHeaderValue("Basic", credentialsBase64);
            return result;
        }

        protected T GetService<T>() where T : class
        {
            return _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            // remove EF Core registration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BooksDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BooksDbContext, BooksDbContext>(
                (sp, options) =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var testConnectionString = configuration.GetValue<string>("ConnectionStrings:BooksDatabase");
                    var parts = testConnectionString.Split(";");
                    var uniqueDbTestConnectionStringBuilder = new StringBuilder();
                    foreach (var part in parts)
                    {
                        var isDatabasePart = part.StartsWith("Database=");
                            uniqueDbTestConnectionStringBuilder.Append(isDatabasePart
                            ? $"Database={_uniqueDatabaseName};"
                            : $"{part};");
                    }

                    var uniqueDbTestConnectionString = uniqueDbTestConnectionStringBuilder.ToString().TrimEnd(';');
                    options.UseSqlServer(uniqueDbTestConnectionString);
                });
        }

        protected override async Task Cleanup()
        {
            await base.Cleanup();
            await using var dbContext = _serviceProvider.GetRequiredService<BooksDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
