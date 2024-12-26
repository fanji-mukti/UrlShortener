namespace UrlShortener.Core.Repositories.Entities
{
    /// <summary>
    /// Specifies the type of document stored in the database.
    /// </summary>
    internal enum DocumentType
    {
        /// <summary>
        /// The document type is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The document represents an original URL.
        /// </summary>
        OriginalUrl,

        /// <summary>
        /// The document represents a shortened URL.
        /// </summary>
        ShortenedUrl,
    }
}
