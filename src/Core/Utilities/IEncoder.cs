namespace UrlShortener.Core.Utilities
{
    /// <summary>
    /// Provides an interface for encoding numbers.
    /// </summary>
    public interface IEncoder
    {
        /// <summary>
        /// Encodes the specified number into a string.
        /// </summary>
        /// <param name="number">The number to encode.</param>
        /// <returns>A string representation of the encoded number.</return
        string Encode(long number);
    }
}
