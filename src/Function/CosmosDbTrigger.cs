namespace UrlShortener.Function
{
    using EnsureThat;
    using Microsoft.Azure.Functions.Worker;
    using System.Collections.Generic;
    using System.Linq;
    using UrlShortener.Core.Models;
    using UrlShortener.Core.Repositories;

    internal class CosmosDbTrigger
    {
        private readonly IUrlRepository _urlRepository;

        public CosmosDbTrigger(IUrlRepository urlRepository)
        {
            _urlRepository = EnsureArg.IsNotNull(urlRepository, nameof(urlRepository));
        }

        [Function("CosmosTrigger")]
        [CosmosDBOutput("UrlShortenerStore", "ShortenedUrl", Connection = "CosmosDb.ConnectionString")]
        public async Task RunAsync([CosmosDBTrigger(
            databaseName: "UrlShortenerStore",
            containerName:"ShortenedUrl",
            Connection = "CosmosDb.ConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<ShortenedUrl> shortenedUrls)
        {
            if (shortenedUrls is null || !shortenedUrls.Any())
            {
                return;
            }

            var addToLookupTasks = shortenedUrls.Select(_urlRepository.AddToLookupAsync);
            await Task.WhenAll(addToLookupTasks).ConfigureAwait(false);
        }
    }
}
