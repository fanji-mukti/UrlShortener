namespace UrlShortener.Core.UnitTests.Services
{
    public sealed class SnowflakeIdGeneratorTests
    {
        private readonly SnowflakeIdGeneratorSteps _steps = new SnowflakeIdGeneratorSteps();
        public static IEnumerable<object[]> TestData => new List<object[]>
        {
            new object[] { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 135168L },
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GenerateId_ShouldReturnUniqueId(
            long dataCenterId,
            long workerId,
            DateTime generatedAt,
            long expected)
        {
            _steps
                .Given_A_TimeProvider_With(dataCenterId, workerId)
                .Given_The_Time_Is(generatedAt)
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
