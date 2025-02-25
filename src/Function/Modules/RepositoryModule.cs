﻿namespace UrlShortener.Function.Modules
{
    using Autofac;
    using EnsureThat;
    using Microsoft.Azure.Cosmos;
    using UrlShortener.Core.Repositories;
    using UrlShortener.Function.Configurations;

    /// <summary>
    /// Represents a module for registering repository dependencies in the Autofac container.
    /// </summary>
    internal sealed class RepositoryModule : Module
    {
        private readonly CosmosDbConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryModule"/> class with the specified Cosmos DB configuration.
        /// </summary>
        /// <param name="configuration">The configuration settings for connecting to the Cosmos DB instance.</param>
        public RepositoryModule(CosmosDbConfiguration configuration)
        {
            _configuration = EnsureArg.IsNotNull(configuration, nameof(configuration));
        }

        /// <summary>
        /// Adds registrations to the Autofac container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(_configuration)
                .AsSelf()
                .SingleInstance();

            builder
                .Register(context =>
                {
                    var config = context.Resolve<CosmosDbConfiguration>();
                    var cosmosClientOptions = new CosmosClientOptions
                    {
                        SerializerOptions = new CosmosSerializationOptions
                        {
                            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                        },
                        ConnectionMode = (ConnectionMode)Enum.Parse(typeof(ConnectionMode), config.ConnectionMode),
                    };

                    var client = new CosmosClient(config.ConnectionString, cosmosClientOptions);

                    return client;
                })
                .SingleInstance();

            builder
                .RegisterType<CosmosDbUrlRepository>()
                .As<IUrlRepository>()
                .SingleInstance();
        }
    }
}
