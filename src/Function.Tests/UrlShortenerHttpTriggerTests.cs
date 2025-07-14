namespace Function.Tests
{
    using Autofac;
    using UrlShortener.Function.DTOs;
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
        public async Task ShortenUrl_WithValidRequest_ShouldBeAbleToRedirectToOriginalUrlWithTheShortenedUrl()
        {
            var shortenUrlRequest = new ShortenUrlRequest
            {
                OriginalUrl = "https://www.example.com/",
            };

            await this.steps
                .WhenShortenUrlAsyncIsCalled(shortenUrlRequest)
                .ConfigureAwait(true);

            await this.steps
                .WhenShortUrlAsyncIsCalledWithTheShortenedUrl()
                .ConfigureAwait(true);

            this.steps
                .ThenResultShouldBeRedirectResult(shortenUrlRequest.OriginalUrl);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalid-url-format")]
        public async Task ShortenUrl_WithInvalidRequest_ShouldReturnBadRequest(string originalUrl)
        {
            var shortenUrlRequest = new ShortenUrlRequest
            {
                OriginalUrl = originalUrl,
            };

            await this.steps
                .WhenShortenUrlAsyncIsCalled(shortenUrlRequest)
                .ConfigureAwait(true);

            this.steps
                .ThenResultShouldBeBadRequestObjectResult();
        }

        [Fact]
        public async Task ShortUrl_NotExistsShortUrl_ShouldReturnNotFound()
        { 
            await this.steps
                .WhenShortUrlAsyncIsCalled("non-existing-short-url")
                .ConfigureAwait(true);

            this.steps
                .ThenResultShouldBeNotFoundResult();
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
