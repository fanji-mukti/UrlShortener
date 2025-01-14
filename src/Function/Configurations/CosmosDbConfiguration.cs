namespace UrlShortener.Function.Configurations
{
    using System.Net;
    using System.Security;

    public sealed class CosmosDbConfiguration : IDisposable
    {
        private SecureString? _secureConnectionString;
        private bool _disposed;

        public required string ConnectionString 
        {
            get
            {
                return _secureConnectionString == null ? string.Empty : new NetworkCredential(string.Empty, _secureConnectionString).Password;
            }
            set
            {
                _secureConnectionString?.Dispose();
                _secureConnectionString = new NetworkCredential(string.Empty, new SecureString()).SecurePassword;
            } 
        }

        public required string ConnectionMode { get; set; }

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
