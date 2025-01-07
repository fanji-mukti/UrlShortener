namespace UrlShortener.Core.UnitTests.Steps
{
    using Moq;
    using System;
    using System.Threading.Tasks;
    using UrlShortener.Core.Models;
    using UrlShortener.Core.Repositories;
    using UrlShortener.Core.Services;
    using UrlShortener.Core.Utilities;
    using FluentAssertions;
    using Xunit;

    internal sealed class UrlShortenerServiceSteps
    {
        private readonly Mock<IUrlRepository> _urlRepositoryMock = new Mock<IUrlRepository>();
        private readonly Mock<IIdGenerator> _idGeneratorMock = new Mock<IIdGenerator>();
        private readonly Mock<IEncoder> _encoderMock = new Mock<IEncoder>();
        private UrlShortenerService _service;
        private string _originalUrl;
        private string _shortUrl;
        private DateTime? _expiresAt;
        private ShortenedUrl? _result;
        private Exception _exception;

        public UrlShortenerServiceSteps GivenAUrlShortenerService()
        {
            _service = new UrlShortenerService(_urlRepositoryMock.Object, _idGeneratorMock.Object, _encoderMock.Object);
            return this;
        }

        public UrlShortenerServiceSteps GivenAnOriginalUrl(string originalUrl)
        {
            _originalUrl = originalUrl;
            return this;
        }

        public UrlShortenerServiceSteps GivenAnExistingShortenedUrl(ShortenedUrl shortenedUrl)
        {
            _urlRepositoryMock
                .Setup(repo => repo.GetByOriginalUrlAsync(_originalUrl))
                .ReturnsAsync(shortenedUrl);
            
            return this;
        }

        public UrlShortenerServiceSteps GivenANewShortUrl(string shortUrl)
        {
            _shortUrl = shortUrl;

            _idGeneratorMock
                .Setup(gen => gen.GenerateId())
                .Returns(1110011);

            _encoderMock
                .Setup(enc => enc.Encode(It.IsAny<long>()))
                .Returns(shortUrl);

            return this;
        }

        public UrlShortenerServiceSteps GivenAnExpirationDate(DateTime? expiresAt)
        {
            _expiresAt = expiresAt;
            return this;
        }

        public async Task<UrlShortenerServiceSteps> WhenShortenUrlIsCalled()
        {
            _exception = await Record
                .ExceptionAsync(async () => _result = await _service.ShortenUrlAsync(_originalUrl, _expiresAt))
                .ConfigureAwait(false);

            return this;
        }

        public async Task<UrlShortenerServiceSteps> WhenGetOriginalUrlIsCalled()
        {
            _exception = await Record
                .ExceptionAsync(async () => _result = await _service.GetOriginalUrlAsync(_shortUrl))
                .ConfigureAwait(false);

            return this;
        }

        public UrlShortenerServiceSteps ThenAnExceptionShouldBeThrown<TException>() where TException : Exception
        {
            _exception.Should().BeOfType<TException>();
            return this;
        }

        public UrlShortenerServiceSteps ThenTheResultShouldBe(ShortenedUrl? expected)
        {
            _result.Should().BeEquivalentTo(expected, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .When(info => info.Path.EndsWith(nameof(ShortenedUrl.CreatedAt))));

            return this;
        }

        public UrlShortenerServiceSteps ThenTheShortenedUrlShouldBeAddedToRepository()
        {
            _urlRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ShortenedUrl>()), Times.Once);
            return this;
        }
    }
}
