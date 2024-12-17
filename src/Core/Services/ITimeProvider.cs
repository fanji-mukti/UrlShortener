namespace UrlShortener.Core.Services
{
    /// <summary>
    /// Provides an abstraction for retrieving the current time in milliseconds.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Gets the current time in milliseconds since the Unix epoch.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        long GetCurrentTimeMilliseconds();
    }
}
