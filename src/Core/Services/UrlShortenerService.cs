namespace UrlShortener.Core.Services
{
    using EnsureThat;
    using System;
    using System.Threading.Tasks;
    using UrlShortener.Core.Models;
    using UrlShortener.Core.Repositories;
    using UrlShortener.Core.Utilities;

    /// <summary>
    /// Service for shortening URLs and retrieving original URLs.
    /// </summary>
    public sealed class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IIdGenerator _idGenerator;
        private readonly IEncoder _encoder;
        private readonly string _baseAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlShortenerService"/> class.
        /// </summary>
        /// <param name="urlRepository">The URL repository.</param>
        /// <param name="idGenerator">The ID generator.</param>
        /// <param name="encoder">The encoder of the id.</param>
        public UrlShortenerService(
            IUrlRepository urlRepository,
            IIdGenerator idGenerator,
            IEncoder encoder,
            string baseAddress)
        {
            _urlRepository = EnsureArg.IsNotNull(urlRepository, nameof(urlRepository));
            _idGenerator = EnsureArg.IsNotNull(idGenerator, nameof(idGenerator));
            _encoder = EnsureArg.IsNotNull(encoder, nameof(encoder));
            _baseAddress = EnsureArg.IsNotNullOrWhiteSpace(baseAddress, nameof(baseAddress));
        }

        /// <summary>
        /// Shortens the specified original URL.
        /// </summary>
        /// <param name="originalUrl">The original URL to be shortened.</param>
        /// <param name="expiresAt">The optional expiration date and time for the shortened URL.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the shortened URL.</returns>
        /// <exception cref="ArgumentException">Thrown when the original URL is null or empty.</exception>
        public async Task<ShortenedUrl> ShortenUrlAsync(string originalUrl, DateTime? expiresAt)
        {
            EnsureArg.IsNotNullOrWhiteSpace(originalUrl, nameof(originalUrl));

            var existingUrl = await _urlRepository.GetByOriginalUrlAsync(originalUrl);
            if (existingUrl != null)
            {
                return existingUrl;
            }

            var id = _idGenerator.GenerateId();
            var shortUrl = _encoder.Encode(id);
            var shortenedUrl = new ShortenedUrl(originalUrl, shortUrl, DateTime.UtcNow, expiresAt);

            await _urlRepository.AddAsync(shortenedUrl).ConfigureAwait(false);

            return AppendBaseAddress(shortenedUrl);

            ShortenedUrl AppendBaseAddress(ShortenedUrl original)
            {
                return new ShortenedUrl(
                    original.OriginalUrl,
                    $"{_baseAddress}/{original.ShortUrl}",
                    original.CreatedAt,
                    original.ExpiresAt);
            }
        }

        /// <summary>
        /// Gets the original URL for the specified short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the original URL.</returns>
        /// <exception cref="ArgumentException">Thrown when the short URL is null or empty.</exception>
        public async Task<ShortenedUrl?> GetOriginalUrlAsync(string shortUrl)
        {
            EnsureArg.IsNotNull(shortUrl, nameof(shortUrl));

            var shortenedUrl = await _urlRepository.GetAsync(shortUrl).ConfigureAwait(false);
            if (shortenedUrl == null)
            {
                return null;
            }

            return shortenedUrl;
        }
    }
}
