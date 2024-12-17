namespace UrlShortener.Core.UnitTests.Services
{
    using Moq;
    using UrlShortener.Core.Services;
    using Xunit;

    public class SnowflakeIdGeneratorSteps
    {
        private Mock<ITimeProvider> _mockTimeProvider;
        private SnowflakeIdGenerator _generator;
        private long _generatedId;
        private long _expectedTime;

        public void Given_A_TimeProvider_And_SnowflakeIdGenerator()
        {
            _mockTimeProvider = new Mock<ITimeProvider>();
            _expectedTime = 1640995200000L; // Example timestamp
            _mockTimeProvider.Setup(tp => tp.GetCurrentTimeMilliseconds()).Returns(_expectedTime);

            _generator = new SnowflakeIdGenerator(1, 1, _mockTimeProvider.Object);
        }

        public void When_Generating_An_Id()
        {
            _generatedId = _generator.GenerateId();
        }

        public void Then_The_Id_Should_Be_Unique()
        {
            Assert.NotNull(_generatedId);
            // Additional assertions can be added here to verify the structure of the ID
        }

        public void Then_The_Id_Should_Be_TimeBased()
        {
            long timestampPart = (_generatedId >> (5 + 5 + 12));
            long expectedTimestampPart = (_expectedTime - 1640995200000L); // Subtract custom epoch
            Assert.Equal(expectedTimestampPart, timestampPart);
        }
    }
}
