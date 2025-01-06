namespace UrlShortener.Core.Repositories
{
    using System.IO.Hashing;
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

            var ttl = CalculateTtl(shortenedUrl.ExpiresAt);
            var document = new ShortenedUrlDocument(
                shortenedUrl.ShortUrl,
                shortenedUrl.ShortUrl,
                shortenedUrl.OriginalUrl,
                shortenedUrl.ShortUrl,
                shortenedUrl.CreatedAt,
                shortenedUrl.ExpiresAt,
                DocumentType.ShortenedUrl,
                ttl);

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
                var hashedOriginalUrl = ComputeCrc32Hash(originalUrl);
                var response = await _container
                 .ReadItemAsync<ShortenedUrlDocument>(hashedOriginalUrl, new PartitionKey(hashedOriginalUrl))
                 .ConfigureAwait(false);

                return ConvertToShortenedUrl(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            static string ComputeCrc32Hash(string input)
            {
                // Check if the input is null or empty
                if (string.IsNullOrEmpty(input))
                {
                    throw new ArgumentException("Input cannot be null or empty", nameof(input));
                }

                // Convert the input string to a byte array
                byte[] bytes = Encoding.UTF8.GetBytes(input);

                // Compute the CRC32 hash
                byte[] hashBytes = Crc32.Hash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private static int CalculateTtl(DateTime? expiresAt)
        {
            if (expiresAt == null)
            {
                return -1;
            }

            var ttl = (int)(expiresAt.Value - DateTime.UtcNow).TotalSeconds;
            return ttl > 0 ? ttl : 0; // Ensure TTL is non-negative
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
