namespace UrlShortener.Core.Repositories.Entities
{
    /// <summary>
    /// Represents a shortened URL document stored in the database.
    /// </summary>
    /// <param name="Id">The unique identifier for the URL document.</param>
    /// <param name="PartitionKey">The partition key of the document, used for efficient querying in Cosmos DB.</param>
    /// <param name="OriginalUrl">The original URL that is being shortened.</param>
    /// <param name="ShortUrl">The shortened URL.</param>
    /// <param name="CreatedAt">The date and time when the URL was created.</param>
    /// <param name="ExpiresAt">The optional expiration date and time for the URL.</param>
    internal record ShortenedUrlDocument(
        string Id,
        string PartitionKey,
        string OriginalUrl,
        string ShortUrl,
        DateTime CreatedAt,
        DateTime? ExpiresAt
    );
}
