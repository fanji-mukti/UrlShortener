namespace UrlShortener.Core.Repositories
{
    using UrlShortener.Core.Models;

    /// <summary>
    /// Interface for URL repository to manage shortened URLs.
    /// </summary>
    public interface IUrlRepository
    {
        /// <summary>
        /// Adds a new shortened URL to the repository.
        /// </summary>
        /// <param name="shortenedUrl">The shortened URL to add.</param>
        /// <returns>The added shortened URL.</returns>
        Task<ShortenedUrl> AddAsync(ShortenedUrl shortenedUrl);

        /// <summary>
        /// Adds a new shortened URL to the lookup table.
        /// </summary>
        /// <param name="shortenedUrl">The shortened URL to add.</param>
        /// <returns>The added shortened URL.</returns>
        Task<ShortenedUrl> AddToLookupAsync(ShortenedUrl shortenedUrl);

        /// <summary>
        /// Retrieves a shortened URL from the repository by its short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to retrieve.</param>
        /// <returns>The shortened URL if found; otherwise, null.</returns>
        Task<ShortenedUrl?> GetAsync(string shortUrl);

        /// <summary>
        /// Retrieves a shortened URL from the repository by its original URL.
        /// </summary>
        /// <param name="originalUrl">The original URL to retrieve.</param>
        /// <returns>The shortened URL if found; otherwise, null.</returns>
        Task<ShortenedUrl?> GetByOriginalUrlAsync(string originalUrl);
    }
}
