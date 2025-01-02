namespace UrlShortener.Core.UnitTests.Repositories
{
    using System;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Azure.Cosmos;
    using Moq;
    using UrlShortener.Core.Models;
    using UrlShortener.Core.Repositories;
    using UrlShortener.Core.Repositories.Entities;

    internal sealed class CosmosDbUrlRepositorySteps
    {
        private readonly Mock<Container> _containerMock = new Mock<Container>();
        private CosmosDbUrlRepository _repository;
        private ShortenedUrl _shortenedUrl;
        private ShortenedUrl _result;

        public CosmosDbUrlRepositorySteps()
        {
            _repository = new CosmosDbUrlRepository(_containerMock.Object);
        }

        public CosmosDbUrlRepositorySteps GivenAValidShortenedUrl()
        {
            _shortenedUrl = new ShortenedUrl("https://original.url", "shortUrl", DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            var document = new ShortenedUrlDocument(
                "shortUrl",
                "shortUrl",
                "https://original.url",
                "shortUrl",
                _shortenedUrl.CreatedAt,
                _shortenedUrl.ExpiresAt,
                DocumentType.ShortenedUrl);

            var responseMock = new Mock<ItemResponse<ShortenedUrlDocument>>();
            responseMock.Setup(r => r.Resource).Returns(document);

            _containerMock
                .Setup(c => c.CreateItemAsync(It.IsAny<ShortenedUrlDocument>(), It.IsAny<PartitionKey>(), null, default))
                .ReturnsAsync(responseMock.Object);
            
                return this;
        }

        public CosmosDbUrlRepositorySteps GivenAnExistingShortenedUrl()
        {
            _shortenedUrl = new ShortenedUrl("https://original.url", "shortUrl", DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
            var document = new ShortenedUrlDocument(
                "shortUrl",
                "shortUrl",
                "https://original.url",
                "shortUrl",
                _shortenedUrl.CreatedAt,
                _shortenedUrl.ExpiresAt,
                DocumentType.ShortenedUrl);
            
            var responseMock = new Mock<ItemResponse<ShortenedUrlDocument>>();
            responseMock
                .Setup(r => r.Resource)
                .Returns(document);

            _containerMock
                .Setup(c => c.ReadItemAsync<ShortenedUrlDocument>("shortUrl", new PartitionKey("shortUrl"), null, default))
                .ReturnsAsync(responseMock.Object);

            return this;
        }

        public CosmosDbUrlRepositorySteps GivenANonExistingShortenedUrl()
        {
            _containerMock
                .Setup(c => c.ReadItemAsync<ShortenedUrlDocument>("shortUrl", new PartitionKey("shortUrl"), null, default))
                .ThrowsAsync(new CosmosException("Not Found", System.Net.HttpStatusCode.NotFound, 0, "", 0));

            return this;
        }

        public CosmosDbUrlRepositorySteps GivenAnExistingShortenedUrlByOriginalUrl()
        {
            _shortenedUrl = new ShortenedUrl("https://original.url", "shortUrl", DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
            var document = new ShortenedUrlDocument("shortUrl", "shortUrl", "https://original.url", "shortUrl", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), DocumentType.ShortenedUrl);
            
            var responseMock = new Mock<ItemResponse<ShortenedUrlDocument>>();
            responseMock
                .Setup(r => r.Resource)
                .Returns(document);

            _containerMock
                .Setup(c => c.ReadItemAsync<ShortenedUrlDocument>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default))
                .ReturnsAsync(responseMock.Object);

            return this;
        }

        public CosmosDbUrlRepositorySteps GivenANonExistingShortenedUrlByOriginalUrl()
        {
            _containerMock
                .Setup(c => c.ReadItemAsync<ShortenedUrlDocument>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default))
                .ThrowsAsync(new CosmosException("Not Found", System.Net.HttpStatusCode.NotFound, 0, "", 0));

            return this;
        }

        public async Task<CosmosDbUrlRepositorySteps> WhenAddAsyncIsCalled()
        {
            _result = await _repository.AddAsync(_shortenedUrl);
            return this;
        }

        public async Task<CosmosDbUrlRepositorySteps> WhenGetAsyncIsCalled()
        {
            _result = await _repository.GetAsync("shortUrl");
            return this;
        }

        public async Task<CosmosDbUrlRepositorySteps> WhenGetByOriginalUrlAsyncIsCalled()
        {
            _result = await _repository.GetByOriginalUrlAsync("https://original.url");
            return this;
        }

        public void ThenTheShortenedUrlShouldBeAdded()
        {
            _result.Should().NotBeNull();
            _result.Should().BeEquivalentTo(_shortenedUrl);
        }

        public void ThenTheShortenedUrlShouldBeReturned()
        {
            _result.Should().NotBeNull();
            _result.ShortUrl.Should().Be(_shortenedUrl.ShortUrl);
        }

        public void ThenNullShouldBeReturned()
        {
            _result.Should().BeNull();
        }
    }
}
