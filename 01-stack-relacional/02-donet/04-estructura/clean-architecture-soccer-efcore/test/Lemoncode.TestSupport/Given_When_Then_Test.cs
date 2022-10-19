using System;

namespace Lemoncode.TestSupport
{
    public abstract class Given_When_Then_Test
        : IDisposable
    {
        public void Dispose()
        {
            Cleanup();
        }

        protected Given_When_Then_Test()
        {
            Setup();
        }

        private void Setup()
        {
            Given();
            When();
        }

        protected abstract void Given();

        protected abstract void When();

        protected virtual void Cleanup()
        {
        }
    }

}
