namespace Function.Tests
{
    using Autofac;
    using UrlShortener.Core.Models;

    [Collection(CosmosDbCollection.Name)]
    public sealed class CosmosDbTriggerTests : IDisposable
    {
        private readonly CosmosDbTriggerSteps steps;
        private readonly ILifetimeScope lifetimeScope;
        private bool disposed;

        public CosmosDbTriggerTests(TestFixture fixture)
        {
            this.lifetimeScope = fixture.Container.BeginLifetimeScope();
            this.steps = this.lifetimeScope.Resolve<CosmosDbTriggerSteps>();
        }

        [Fact]
        public async Task RunAsync_WithValidShortenedUrl_ShouldAddToLookup()
        {
            var shortenedUrl = new ShortenedUrl(
                "www.example.com",
                "short",
                new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                null);

            await this.steps
                .WhenICallRunAsync(shortenedUrl)
                .ConfigureAwait(true);

            await this.steps
                .ThenTheUrlShouldBeAddedInTheLookup()
                .ConfigureAwait(true);
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
