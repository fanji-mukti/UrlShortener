namespace UrlShortener.Core.UnitTests.Services
{
    using FluentAssertions;
    using Moq;
    using UrlShortener.Core.Services;

    internal sealed class SnowflakeIdGeneratorSteps
    {
        
        private readonly Mock<ITimeProvider> _mockTimeProvider = new Mock<ITimeProvider>();
        private SnowflakeIdGenerator _generator;
        private long _generatedId;
        private List<long> _generatedIds = new List<long>();
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

        public SnowflakeIdGeneratorSteps Given_There_Are_Id_Generated_At_The_Same_Milliseconds(int generatedIdCount)
        {
            for (int i = 0; i < generatedIdCount; i++)
            {
                this.When_Generating_An_Id();
            }

            return this;
        }

        public SnowflakeIdGeneratorSteps When_Generating_An_Id()
        {
            _generatedId = _generator.GenerateId();
            _generatedIds.Add(_generatedId);
            return this;
        }

        public SnowflakeIdGeneratorSteps Then_The_Id_Should_Be(long expected)
        {
            _generatedId.Should().Be(expected);
            return this;
        }

        public SnowflakeIdGeneratorSteps Then_The_Id_Should_Be_Unique()
        {
            _generatedIds.Should().OnlyHaveUniqueItems();
            return this;
        }

        public SnowflakeIdGeneratorSteps Then_The_Id_Should_Be_TimeBased()
        {
            long timestampPart = (_generatedId >> (5 + 5 + 12));
            long expectedTimestampPart = (_generatedTime - 1735689600000L); // Subtract custom epoch

            timestampPart.Should().Be(expectedTimestampPart);
            return this;
        }
    }
}
