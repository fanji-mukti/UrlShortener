namespace UrlShortener.Core.UnitTests.Repositories
{
    using System.Threading.Tasks;
    using Xunit;

    public class CosmosDbUrlRepositoryTests
    {
        private readonly CosmosDbUrlRepositorySteps _steps = new CosmosDbUrlRepositorySteps();

        [Fact]
        public async Task AddAsync_ShouldAddShortenedUrl()
        {
            await _steps
                .GivenAValidShortenedUrl()
                .WhenAddAsyncIsCalled()
                .ConfigureAwait(true);

            _steps
                .ThenTheShortenedUrlShouldBeAdded();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnShortenedUrl_WhenUrlExists()
        {
            await _steps
                .GivenAnExistingShortenedUrl()
                .WhenGetAsyncIsCalled()
                .ConfigureAwait(true);

            _steps
                .ThenTheShortenedUrlShouldBeReturned();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenUrlDoesNotExist()
        {
            await _steps
                .GivenANonExistingShortenedUrl()
                .WhenGetAsyncIsCalled()
                .ConfigureAwait(true);

            _steps
                .ThenNullShouldBeReturned();
        }

        [Fact]
        public async Task GetByOriginalUrlAsync_ShouldReturnShortenedUrl_WhenUrlExists()
        {
            await _steps
                .GivenAnExistingShortenedUrlByOriginalUrl()
                .WhenGetByOriginalUrlAsyncIsCalled()
                .ConfigureAwait(true);

            _steps
                .ThenTheShortenedUrlShouldBeReturned();
        }

        [Fact]
        public async Task GetByOriginalUrlAsync_ShouldReturnNull_WhenUrlDoesNotExist()
        {
            await _steps
                .GivenANonExistingShortenedUrlByOriginalUrl()
                .WhenGetByOriginalUrlAsyncIsCalled()
                .ConfigureAwait(true);

            _steps
                .ThenNullShouldBeReturned();
        }
    }
}
