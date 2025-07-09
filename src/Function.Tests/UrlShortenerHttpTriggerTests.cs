namespace Function.Tests
{
    using Autofac;
    using Xunit;

    [Collection(CosmosDbCollection.Name)]
    public sealed class UrlShortenerHttpTriggerTests : IDisposable
    {
        private readonly UrlShorternerHttpTriggerSteps steps;
        private readonly ILifetimeScope lifetimeScope;
        private bool disposed;

        public UrlShortenerHttpTriggerTests(TestFixture fixture)
        {
            this.lifetimeScope = fixture.Container.BeginLifetimeScope();
            this.steps = this.lifetimeScope.Resolve<UrlShorternerHttpTriggerSteps>();
        }

        [Fact]
        public void Test()
        {
            var a = this.steps;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.lifetimeScope.Dispose();
            this.disposed = true;
        }
    }
}
