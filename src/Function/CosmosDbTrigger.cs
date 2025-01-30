namespace UrlShortener.Function
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UrlShortener.Core.Repositories.Entities;

    internal class CosmosDbTrigger
    {
        [Function("CosmosTrigger")]
        [CosmosDBOutput("UrlShortenerStore", "ShortenedUrl", Connection = "CosmosDb.ConnectionString")]
        public IReadOnlyList<OriginalUrlDocument>? Run([CosmosDBTrigger(
            databaseName: "UrlShortenerStore",
            containerName:"ShortenedUrl",
            Connection = "CosmosDb.ConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<ShortenedUrlDocument> shortenedUrlDocuments,
            FunctionContext context)
        {
            if (shortenedUrlDocuments is null || !shortenedUrlDocuments.Any())
            {
                return null;
            }

            var output = shortenedUrlDocuments
                .Select(ToOriginalUrlDocument)
                .ToList();

            return output;

            OriginalUrlDocument ToOriginalUrlDocument(ShortenedUrlDocument shortenedUrlDocument)
            { 
                return new OriginalUrlDocument(
                    shortenedUrlDocument.OriginalUrl,
                    shortenedUrlDocument.OriginalUrl,
                    shortenedUrlDocument.OriginalUrl,
                    shortenedUrlDocument.ShortUrl,
                    shortenedUrlDocument.CreatedAt,
                    shortenedUrlDocument.ExpiresAt,
                    shortenedUrlDocument.ttl
                );
            }
        }

        public record OriginalUrlDocument(
            string id,
            string partitionKey,
            string originalUrl,
            string shortUrl,
            DateTime createdAt,
            DateTime? expiresAt,
            int ttl
            );
    }
}
