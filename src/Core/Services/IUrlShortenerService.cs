namespace UrlShortener.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using UrlShortener.Core.Models;

    /// <summary>
    /// Interface for URL shortener service.
    /// </summary>
    public interface IUrlShortenerService
    {
        /// <summary>
        /// Shortens the specified original URL.
        /// </summary>
        /// <param name="originalUrl">The original URL to be shortened.</param>
        /// <param name="expiresAt">The optional expiration date and time for the shortened URL.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the shortened URL.</returns>
        Task<ShortenedUrl> ShortenUrlAsync(string originalUrl, DateTime? expiresAt);

        /// <summary>
        /// Gets the original URL for the specified short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the original URL.</returns>
        Task<ShortenedUrl> GetOriginalUrlAsync(string shortUrl);
    }
}
