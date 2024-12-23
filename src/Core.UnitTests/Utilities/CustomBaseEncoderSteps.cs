namespace UrlShortener.Core.UnitTests.Utilities
{
    using FluentAssertions;
    using UrlShortener.Core.Utilities;

    public class CustomBaseEncoderSteps
    {
        private CustomBaseEncoder _encoder;
        private long _number;
        private string _result;
        private Exception _exception;

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
            _exception = Record.Exception(() => _result = _encoder.Encode(_number));
            return this;
        }

        public CustomBaseEncoderSteps ThenTheResultShouldBe(string expected)
        {
            _result.Should().NotBeNullOrEmpty();
            _result.Should().Be(expected);
            return this;
        }

        public CustomBaseEncoderSteps ThenAnExceptionShouldBeThrown<TException>() where TException : Exception
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<TException>();
            return this;
        }

        public CustomBaseEncoderSteps ThenNoExceptionShouldBeThrown()
        {
            _exception.Should().BeNull();
            return this;
        }
    }
}
