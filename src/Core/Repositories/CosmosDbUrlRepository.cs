namespace UrlShortener.Core.Repositories
{
    using System.Security.Cryptography;
    using System.Text;
    using EnsureThat;
    using Microsoft.Azure.Cosmos;
    using UrlShortener.Core.Models;
    using UrlShortener.Core.Repositories.Entities;

    /// <summary>
    /// Url Repository implementation using Cosmos DB.
    /// </summary>
    public sealed class CosmosDbUrlRepository : IUrlRepository
    {
        private readonly Container _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlRepository"/> class.
        /// </summary>
        /// <param name="container">The <see cref="Container"/>.</param>
        public CosmosDbUrlRepository(Container container)
        {
            _container = EnsureArg.IsNotNull(container, nameof(container));
        }

        /// <summary>
        /// Adds a new shortened URL to the repository.
        /// </summary>
        /// <param name="shortenedUrl">The shortened URL to add.</param>
        /// <returns>The added shortened URL.</returns>
        public async Task<ShortenedUrl> AddAsync(ShortenedUrl shortenedUrl)
        {
            EnsureArg.IsNotNull(shortenedUrl, nameof(shortenedUrl));

            var document = new ShortenedUrlDocument(
                shortenedUrl.ShortUrl,
                shortenedUrl.ShortUrl,
                shortenedUrl.OriginalUrl,
                shortenedUrl.ShortUrl,
                shortenedUrl.CreatedAt,
                shortenedUrl.ExpiresAt,
                DocumentType.ShortenedUrl);

            var response = await _container
                .CreateItemAsync(document, new PartitionKey(document.PartitionKey))
                .ConfigureAwait(false);

            return ConvertToShortenedUrl(response.Resource);
        }

        /// <summary>
        /// Retrieves a shortened URL from the repository by its short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to retrieve.</param>
        /// <returns>The shortened URL if found; otherwise, null.</returns>
        public async Task<ShortenedUrl?> GetAsync(string shortUrl)
        {
            EnsureArg.IsNotNullOrWhiteSpace(shortUrl, nameof(shortUrl));

            try
            {
                var response = await _container
                 .ReadItemAsync<ShortenedUrlDocument>(shortUrl, new PartitionKey(shortUrl))
                 .ConfigureAwait(false);

                return ConvertToShortenedUrl(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a shortened URL from the repository by its original URL.
        /// </summary>
        /// <param name="originalUrl">The original URL to retrieve.</param>
        /// <returns>The shortened URL if found; otherwise, null.</returns>
        public async Task<ShortenedUrl?> GetByOriginalUrlAsync(string originalUrl)
        {
            EnsureArg.IsNotNullOrWhiteSpace(originalUrl, nameof(originalUrl));

            try
            {
                var hashedOriginalUrl = ComputeSha256Hash(originalUrl);
                var response = await _container
                 .ReadItemAsync<ShortenedUrlDocument>(hashedOriginalUrl, new PartitionKey(hashedOriginalUrl))
                 .ConfigureAwait(false);

                return ConvertToShortenedUrl(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            string ComputeSha256Hash(string input)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    // Compute the hash as a byte array
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                    // Convert the byte array to a hexadecimal string
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }

                    return builder.ToString();
                }
            }
        }

        private static ShortenedUrl ConvertToShortenedUrl(ShortenedUrlDocument document)
        {
            return new ShortenedUrl(
                document.OriginalUrl,
                document.ShortUrl,
                document.CreatedAt,
                document.ExpiresAt);
        }
    }
}
