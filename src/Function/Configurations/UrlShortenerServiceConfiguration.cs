namespace UrlShortener.Function.Configurations
{
    /// <summary>
    /// Represents the configuration settings for the URL shortener service.
    /// </summary>
    public sealed class UrlShortenerServiceConfiguration
    {
        /// <summary>
        /// Gets or sets the base URL for the shortened URLs.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the data center identifier.
        /// </summary>
        /// <value>
        /// The identifier for the data center.
        /// </value>
        public long DataCenterId { get; set; }

        /// <summary>
        /// Gets or sets the worker identifier.
        /// </summary>
        /// <value>
        /// The identifier for the worker.
        /// </value>
        public long WorkerId { get; set; }
    }
}
