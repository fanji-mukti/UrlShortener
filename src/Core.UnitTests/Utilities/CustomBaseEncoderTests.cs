namespace UrlShortener.Core.UnitTests.Utilities
{
    using Xunit;

    public class CustomBaseEncoderTests
    {
        private readonly CustomBaseEncoderSteps _steps = new CustomBaseEncoderSteps();

        [Fact]
        public void Encode_ShouldReturnBase64EncodedString_WhenNumberIsPositive()
        {
            _steps
                .GivenAnEncoder()
                .GivenANumber(123456789)
                .WhenEncodingTheNumber()
                .ThenTheResultShouldBe("7KwoJ");
        }

        [Fact]
        public void Encode_ShouldReturnZeroCharacter_WhenNumberIsZero()
        {
            _steps
                .GivenAnEncoder()
                .GivenANumber(0)
                .WhenEncodingTheNumber()
                .ThenTheResultShouldBe("0");
        }

        [Fact]
        public void Encode_ShouldReturnZeroCharacter_WhenNumberIsNegative()
        {
            _steps
                .GivenAnEncoder()
                .GivenANumber(-1)
                .WhenEncodingTheNumber()
                .ThenTheResultShouldBe("0");
        }

        [Fact]
        public void Encode_ShouldReturnCorrectBase64EncodedString_ForLargeNumber()
        {
            _steps
                .GivenAnEncoder()
                .GivenANumber(11234023833735169L)
                .WhenEncodingTheNumber()
                .ThenTheResultShouldBe("IY00V01");
        }
    }
}
