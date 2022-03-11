using System.Threading.Tasks;
using Xunit;

namespace Lemoncode.Books.FunctionalTests.TestSupport
{
    public abstract class Given_When_Then_Test_Async
        : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await Given();
            await When();
        }

        public async Task DisposeAsync()
        {
            await Cleanup();
        }

        protected virtual Task Cleanup()
        {
            return Task.CompletedTask;
        }

        protected abstract Task Given();

        protected abstract Task When();
    }
}
