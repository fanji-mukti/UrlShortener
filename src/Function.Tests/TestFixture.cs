namespace Function.Tests
{
    using Autofac;
    using Function.Tests.Modules;
    using Microsoft.Azure.Cosmos;
    using UrlShortener.Function.Configurations;
    using UrlShortener.Function.Modules;

    public sealed class TestFixture : IDisposable
    {
        private bool disposed;

        public IContainer Container { get; }

        public TestFixture()
        {
            var builder = new ContainerBuilder();

            var cosmosConfig = new CosmosDbConfiguration
            {
                ConnectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                ConnectionMode = "Direct",
            };

            var serviceConfig = new UrlShortenerServiceConfiguration
            {
                BaseUrl = "http://localhost:7110/api",
                DataCenterId = 1,
                WorkerId = 1,
            };

            SetupCosmosDbAsync(cosmosConfig).GetAwaiter().GetResult();

            builder
                .RegisterModule(new RepositoryModule(cosmosConfig))
                .RegisterModule(new UrlShortenerServiceModule(serviceConfig))
                .RegisterModule(new MappingModule())
                .RegisterModule(new FunctionTriggerModule())
                .RegisterModule(new TestStepsModule());

            this.Container = builder.Build();
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.Container.Dispose();
            this.disposed = true;
        }

        private async Task SetupCosmosDbAsync(CosmosDbConfiguration config)
        { 
            var client = new CosmosClient(config.ConnectionString);
            var response = await client.CreateDatabaseIfNotExistsAsync("UrlShortenerStore").ConfigureAwait(false);
            var database = response.Database;

            await database.CreateContainerIfNotExistsAsync("ShortenedUrl", "/partitionKey").ConfigureAwait(false);
            await database.CreateContainerIfNotExistsAsync("OriginalUrl", "/partitionKey").ConfigureAwait(false);
        }
    }
}
