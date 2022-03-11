using System;
using System.Net.Http;
using System.Threading.Tasks;
using Lemoncode.Soccer.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Lemoncode.Soccer.IntegrationTests.TestSupport
{
    public abstract class FunctionalTest
        : IAsyncLifetime
    {
        private readonly IServiceProvider _serviceProvider;
        protected HttpClient HttpClient { get; }
        protected IConfiguration Configuration { get; }
        protected FunctionalTest()
        {
            var server =
                new TestServer(
                    new WebHostBuilder()
                        .UseStartup<Startup>()
                        .UseEnvironment("Test")
                        .ConfigureTestServices(ConfigureTestServices));

            HttpClient = server.CreateClient();
            _serviceProvider = server.Services;
            Configuration = _serviceProvider.GetService<IConfiguration>();
        }

        protected T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
        }

        protected virtual void Cleanup()
        {
        }

        protected abstract Task Given();

        protected abstract Task When();

        public async Task InitializeAsync()
        {
            await Given();
            await When();
        }

        public Task DisposeAsync()
        {
            Cleanup();
            return Task.CompletedTask;
        }
    }

}