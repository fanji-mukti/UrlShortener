namespace UrlShortener.Core.Services
{
    /// <summary>
    /// Provides a contract for generating unique identifiers.
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Generates a unique identifier.
        /// </summary>
        /// <returns>A unique identifier as a long value.</returns>
        long GenerateId();
    }
}
