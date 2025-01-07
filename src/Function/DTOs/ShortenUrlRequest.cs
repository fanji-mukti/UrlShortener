namespace UrlShortener.Function.DTOs
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the request to shorten a URL.
    /// </summary>
    public sealed class ShortenUrlRequest
    {
        /// <summary>
        /// Gets or sets the original URL.
        /// </summary>
        /// <value>The original URL to be shortened.</value>
        [Required]
        [Url]
        public string OriginalUrl { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the shortened URL expires, if any.
        /// </summary>
        /// <value>The expiration date and time of the shortened URL, if applicable.</value>
        public DateTime? ExpiresAt { get; set; }
    }
}
