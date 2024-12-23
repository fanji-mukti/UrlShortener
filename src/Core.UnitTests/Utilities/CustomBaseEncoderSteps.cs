namespace UrlShortener.Core.UnitTests.Utilities
{
    using FluentAssertions;
    using UrlShortener.Core.Utilities;

    public class CustomBaseEncoderSteps
    {
        private CustomBaseEncoder _encoder;
        private long _number;
        private string _result;

        public CustomBaseEncoderSteps GivenAnEncoder()
        {
            _encoder = new CustomBaseEncoder();
            return this;
        }

        public CustomBaseEncoderSteps GivenANumber(long number)
        {
            _number = number;
            return this;
        }

        public CustomBaseEncoderSteps WhenEncodingTheNumber()
        {
            _result = _encoder.Encode(_number);
            return this;
        }

        public void ThenTheResultShouldBe(string expected)
        {
            _result.Should().NotBeNullOrEmpty();
            _result.Should().Be(expected);
        }
    }
}
