namespace Function.Tests
{
    using FluentAssertions;
    using UrlShortener.Core.Models;
    using UrlShortener.Core.Repositories;
    using UrlShortener.Function;

    public sealed class CosmosDbTriggerSteps
    {
        private readonly CosmosDbTrigger _function;
        private readonly IUrlRepository _urlRepository;
        private ShortenedUrl _shortenedUrl;

        public CosmosDbTriggerSteps(CosmosDbTrigger trigger, IUrlRepository urlRepository)
        {
            ArgumentNullException.ThrowIfNull(trigger, nameof(trigger));
            ArgumentNullException.ThrowIfNull(urlRepository, nameof(urlRepository));

            _function = trigger;
            _urlRepository = urlRepository;
        }

        public Task WhenICallRunAsync(ShortenedUrl shortenedUrl)
        { 
            this._shortenedUrl = shortenedUrl;
            return _function.RunAsync(new List<ShortenedUrl> { shortenedUrl });
        }

        public async Task ThenTheUrlShouldBeAddedInTheLookup()
        {
            var shortenedUrl = await _urlRepository.GetByOriginalUrlAsync(_shortenedUrl.OriginalUrl).ConfigureAwait(false);
            shortenedUrl.Should().BeEquivalentTo(_shortenedUrl);
        }
    }
}
