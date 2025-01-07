namespace UrlShortener.Core.UnitTests.Services
{
    using System;
    using System.Threading.Tasks;
    using UrlShortener.Core.Models;
    using UrlShortener.Core.UnitTests.Steps;
    using Xunit;

    public sealed class UrlShortenerServiceTests
    {
        private readonly UrlShortenerServiceSteps _steps = new UrlShortenerServiceSteps();

        [Fact]
        public async Task ShortenUrlAsync_ShouldReturnExistingShortenedUrl_WhenUrlAlreadyExists()
        {
            var originalUrl = "https://example.com";
            var existingShortenedUrl = new ShortenedUrl(originalUrl, "abc123", DateTime.UtcNow, DateTime.UtcNow.AddDays(30));

            _steps
                .GivenAUrlShortenerService()
                .GivenAnOriginalUrl(originalUrl)
                .GivenAnExistingShortenedUrl(existingShortenedUrl);

            await _steps.WhenShortenUrlIsCalled().ConfigureAwait(true);

            _steps.ThenTheResultShouldBe(existingShortenedUrl);
        }

        [Fact]
        public async Task ShortenUrlAsync_ShouldGenerateNewShortenedUrl_WhenUrlDoesNotExist()
        {
            var originalUrl = "https://example.com";
            var shortUrl = "abc123";
            var expiresAt = DateTime.UtcNow.AddDays(30);
            var expectedShortenedUrl = new ShortenedUrl(originalUrl, shortUrl, DateTime.UtcNow, expiresAt);

           _steps
                .GivenAUrlShortenerService()
                .GivenAnOriginalUrl(originalUrl)
                .GivenANewShortUrl(shortUrl)
                .GivenAnExpirationDate(expiresAt);

            await _steps.WhenShortenUrlIsCalled().ConfigureAwait(true);

            _steps
                .ThenTheResultShouldBe(expectedShortenedUrl)
                .ThenTheShortenedUrlShouldBeAddedToRepository();
        }

        [Fact]
        public async Task ShortenUrlAsync_ShouldThrowArgumentNullException_WhenOriginalUrlIsNull()
        {
            _steps
                .GivenAUrlShortenerService()
                .GivenAnOriginalUrl(null);

            await _steps.WhenShortenUrlIsCalled().ConfigureAwait(false);

            _steps.ThenAnExceptionShouldBeThrown<ArgumentNullException>();
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldThrowArgumentException_WhenShortUrlIsNull()
        {
            _steps
                .GivenAUrlShortenerService()
                .GivenANewShortUrl(null);

            await _steps.WhenGetOriginalUrlIsCalled().ConfigureAwait(true);

            _steps.ThenAnExceptionShouldBeThrown<ArgumentNullException>();
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldReturnNull_WhenShortUrlDoesNotExist()
        {
            var shortUrl = "nonexistent";
            _steps
                .GivenAUrlShortenerService()
                .GivenANewShortUrl(shortUrl);

            await _steps.WhenGetOriginalUrlIsCalled().ConfigureAwait(true);

            _steps.ThenTheResultShouldBe(null);
        }
    }
}
