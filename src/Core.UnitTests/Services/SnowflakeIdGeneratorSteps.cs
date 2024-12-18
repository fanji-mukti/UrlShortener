namespace UrlShortener.Core.UnitTests.Services
{
    using Moq;
    using UrlShortener.Core.Services;
    using Xunit;

    public sealed class SnowflakeIdGeneratorSteps
    {
        private Mock<ITimeProvider> _mockTimeProvider = new Mock<ITimeProvider>();
        private SnowflakeIdGenerator _generator;
        private long _generatedId;
        private long _generatedTime;

        public SnowflakeIdGeneratorSteps Given_A_TimeProvider_With(long datacenterId, long workerId)
        { 
            _generator = new SnowflakeIdGenerator(datacenterId, workerId, _mockTimeProvider.Object);
            return this;
        }

        public SnowflakeIdGeneratorSteps Given_The_Time_Is(DateTime datetime)
        {
            _generatedTime = new DateTimeOffset(datetime).ToUnixTimeMilliseconds();

            _mockTimeProvider
                .Setup(tp => tp.GetCurrentTimeMilliseconds())
                .Returns(_generatedTime);

            return this;
        }

        public SnowflakeIdGeneratorSteps When_Generating_An_Id()
        {
            _generatedId = _generator.GenerateId();
            return this;
        }

        public void Then_The_Id_Should_Be(long expected)
        {
            Assert.NotNull(_generatedId);
            // Additional assertions can be added here to verify the structure of the ID
        }

        public void Then_The_Id_Should_Be_TimeBased()
        {
            long timestampPart = (_generatedId >> (5 + 5 + 12));
            long expectedTimestampPart = (_generatedTime - 1640995200000L); // Subtract custom epoch
            Assert.Equal(expectedTimestampPart, timestampPart);
        }
    }
}
