namespace UrlShortener.Function.DTOs
{
    /// <summary>
    /// Represents the response containing the shortened URL details.
    /// </summary>
    public sealed class ShortenedUrlResponse
    {
        /// <summary>
        /// Gets or sets the original URL.
        /// </summary>
        public string OriginalUrl { get; set; }

        /// <summary>
        /// Gets or sets the shortened URL.
        /// </summary>
        public string ShortUrl { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the URL was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the shortened URL expires, if any.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
    }
}
