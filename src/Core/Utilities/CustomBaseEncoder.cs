namespace UrlShortener.Core.Utilities
{
    using System.Text;

    /// <summary>
    /// Provides methods to encode numbers using a custom base encoding scheme.
    /// </summary>
    public sealed class CustomBaseEncoder : IEncoder
    {
        private const string CustomBaseChars = "0123456789-_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; // 64 characters
        private const int CustomBase = 64;
        private const int EncodedLength = 7;

        /// <summary>
        /// Encodes the specified number into a custom base string.
        /// </summary>
        /// <param name="number">The number to encode.</param>
        /// <returns>A string representation of the encoded number.</returns>
        public string Encode(long number)
        {
            if (number == 0)
            {
                return CustomBaseChars[0].ToString();
            }

            var result = new StringBuilder();
            while (number > 0 && result.Length < EncodedLength)
            {
                result.Insert(0, CustomBaseChars[(int)(number % CustomBase)]);
                number /= CustomBase;
            }

            return result.ToString();
        }
    }
}
