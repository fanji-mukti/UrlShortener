namespace UrlShortener.Core.UnitTests.Utilities
{
    using System;
    using FluentAssertions;
    using Xunit;
    using UrlShortener.Core.Utilities;

    public class CustomBaseEncoderTests
    {
        private readonly CustomBaseEncoder _encoder;

        public CustomBaseEncoderTests()
        {
            _encoder = new CustomBaseEncoder();
        }

        [Fact]
        public void Encode_ShouldReturnBase64EncodedString_WhenNumberIsPositive()
        {
            // Given
            long number = 123456789;

            // When
            string result = _encoder.Encode(number);

            // Then
            result.Should().NotBeNullOrEmpty();
            result.Should().Be("8M0kX");
        }

        [Fact]
        public void Encode_ShouldReturnZeroCharacter_WhenNumberIsZero()
        {
            // Given
            long number = 0;

            // When
            string result = _encoder.Encode(number);

            // Then
            result.Should().NotBeNullOrEmpty();
            result.Should().Be("0");
        }

        [Fact]
        public void Encode_ShouldReturnZeroCharacter_WhenNumberIsNegative()
        {
            // Given
            long number = -1;

            // When
            string result = _encoder.Encode(number);

            // Then
            result.Should().NotBeNullOrEmpty();
            result.Should().Be("0");
        }

        [Fact]
        public void Encode_ShouldReturnCorrectBase64EncodedString_ForLargeNumber()
        {
            // Given
            long number = 9223372036854775807; // Max value for long

            // When
            string result = _encoder.Encode(number);

            // Then
            result.Should().NotBeNullOrEmpty();
            result.Should().Be("FfG7_zzzzzz");
        }
    }
}
