namespace UrlShortener.Core.UnitTests.Services
{
    public sealed class SnowflakeIdGeneratorTests
    {
        private readonly SnowflakeIdGeneratorSteps _steps = new SnowflakeIdGeneratorSteps();

        [Fact]
        public void GenerateId_ShouldReturnUniqueId()
        {
            var dataCenterId = 1;
            var workerId = 1;
            var timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expected = 135168L;
            _steps
                .Given_A_TimeProvider_With(dataCenterId, workerId)
                .Given_The_Time_Is(timestamp)
                .When_Generating_An_Id()
                .Then_The_Id_Should_Be(expected);
        }

        [Fact]
        public void GenerateId_ShouldContainTimeBasedComponent()
        {
            //_steps.Given_A_TimeProvider_And_SnowflakeIdGenerator();
            //_steps.When_Generating_An_Id();
            //_steps.Then_The_Id_Should_Be_TimeBased();
        }
    }
}
