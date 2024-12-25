namespace UrlShortener.Core.Models
{
    /// <summary>
    /// Represents a shortened Url.
    /// </summary>
    /// <param name="OriginalUrl">The original URL that is being shortened.</param>
    /// <param name="ShortUrl">The shortened URL.</param>
    /// <param name="CreatedAt">The date and time when the URL was created.</param>
    /// <param name="ExpiresAt">The optional expiration date and time for the URL.</param>
    public record ShortenedUrl(
        string OriginalUrl,
        string ShortUrl,
        DateTime CreatedAt,
        DateTime? ExpiresAt
    );
}
