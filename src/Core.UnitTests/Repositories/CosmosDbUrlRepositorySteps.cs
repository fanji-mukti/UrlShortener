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
        private ShortenedUrl? _result;

        public CosmosDbUrlRepositorySteps()
        {
            _repository = new CosmosDbUrlRepository(_containerMock.Object);
        }

        public CosmosDbUrlRepositorySteps GivenAValidShortenedUrl()
        {
            _shortenedUrl = CreateShortenedUrl();
            var document = ConvertToShortenedUrlDocument(_shortenedUrl);

            var responseMock = new Mock<ItemResponse<ShortenedUrlDocument>>();
            responseMock.Setup(r => r.Resource).Returns(document);

            _containerMock
                .Setup(c => c.CreateItemAsync(It.IsAny<ShortenedUrlDocument>(), It.IsAny<PartitionKey>(), null, default))
                .ReturnsAsync(responseMock.Object);
            
                return this;
        }

        public CosmosDbUrlRepositorySteps GivenAnExistingShortenedUrl()
        {
            _shortenedUrl = CreateShortenedUrl();
            var document = ConvertToShortenedUrlDocument(_shortenedUrl);

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
            _shortenedUrl = CreateShortenedUrl();
            var ttl = CalculateTtl(_shortenedUrl.ExpiresAt);
            var document = new ShortenedUrlDocument(
                "bf40c5f1",
                "bf40c5f1",
                _shortenedUrl.OriginalUrl,
                _shortenedUrl.ShortUrl,
                _shortenedUrl.CreatedAt,
                _shortenedUrl.ExpiresAt,
                DocumentType.ShortenedUrl,
                ttl);

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
            _result = await _repository.AddAsync(_shortenedUrl).ConfigureAwait(false);
            return this;
        }

        public async Task<CosmosDbUrlRepositorySteps> WhenGetAsyncIsCalled()
        {
            _result = await _repository.GetAsync("shortUrl").ConfigureAwait(false);
            return this;
        }

        public async Task<CosmosDbUrlRepositorySteps> WhenGetByOriginalUrlAsyncIsCalled()
        {
            _result = await _repository.GetByOriginalUrlAsync("https://original.url");
            return this;
        }

        public CosmosDbUrlRepositorySteps ThenTheShortenedUrlShouldBeAdded()
        {
            _result.Should().NotBeNull();
            _result.Should().BeEquivalentTo(_shortenedUrl);
            return this;
        }

        public CosmosDbUrlRepositorySteps ThenTheShortenedUrlShouldBeReturned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeEquivalentTo(_shortenedUrl);
            return this;
        }

        public CosmosDbUrlRepositorySteps ThenNullShouldBeReturned()
        {
            _result.Should().BeNull();
            return this;
        }
    
        private static ShortenedUrl CreateShortenedUrl()
        { 
            return new ShortenedUrl("https://original.url", "shortUrl", DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        }

        private static ShortenedUrlDocument ConvertToShortenedUrlDocument(
            ShortenedUrl shortenedUrl,
            DocumentType documentType = DocumentType.ShortenedUrl)
        {
            var ttl = CalculateTtl(shortenedUrl.ExpiresAt);
            return new ShortenedUrlDocument(
                shortenedUrl.ShortUrl,
                shortenedUrl.ShortUrl,
                shortenedUrl.OriginalUrl,
                shortenedUrl.ShortUrl,
                shortenedUrl.CreatedAt,
                shortenedUrl.ExpiresAt,
                documentType,
                ttl);
        }

        private static int CalculateTtl(DateTime? expiresAt)
        {
            if (expiresAt == null)
            {
                return -1;
            }

            var ttl = (int)(expiresAt.Value - DateTime.UtcNow).TotalSeconds;
            return ttl > 0 ? ttl : 0; // Ensure TTL is non-negative
        }
    }
}
