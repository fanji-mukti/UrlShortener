namespace UrlShortener.Function.Configurations
{
    using System.Net;
    using System.Security;

    /// <summary>
    /// Represents the configuration settings for connecting to an Azure Cosmos DB instance.
    /// </summary>
    public sealed class CosmosDbConfiguration : IDisposable
    {
        private SecureString? _secureConnectionString;
        private bool _disposed;

        /// <summary>
        /// Gets or sets the connection string for the Cosmos DB instance.
        /// </summary>
        /// <value>
        /// The connection string used to connect to the Cosmos DB instance.
        /// </value>
        public required string ConnectionString
        {
            get
            {
                return _secureConnectionString == null ? string.Empty : new NetworkCredential(string.Empty, _secureConnectionString).Password;
            }
            set
            {
                _secureConnectionString?.Dispose();
                _secureConnectionString = new NetworkCredential(string.Empty, value).SecurePassword;
            }
        }

        /// <summary>
        /// Gets or sets the connection mode for the Cosmos DB instance.
        /// </summary>
        /// <value>
        /// The connection mode used to connect to the Cosmos DB instance.
        /// </value>
        public required string ConnectionMode { get; set; }

        /// <summary>
        /// Gets or sets the name of the database in the Cosmos DB instance.
        /// </summary>
        /// <value>
        /// The name of the database in the Cosmos DB instance.
        /// </value>
        public required string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the name of the container in the Cosmos DB instance.
        /// </summary>
        /// <value>
        /// The name of the container in the Cosmos DB instance.
        /// </value>
        public required string ContainerName { get; set; }

        /// <summary>
        /// Releases all resources used by the <see cref="CosmosDbConfiguration"/> class.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _secureConnectionString?.Dispose();
            _disposed = true;
        }
    }
}
