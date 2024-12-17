namespace UrlShortener.Core.Services
{
    /// <summary>
    /// Provides the current time in milliseconds using the system clock.
    /// </summary>
    public class DefaultTimeProvider : ITimeProvider
    {
        /// <summary>
        /// Gets the current time in milliseconds since the Unix epoch.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public long GetCurrentTimeMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
