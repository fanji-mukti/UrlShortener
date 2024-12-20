namespace UrlShortener.Core.UnitTests.Services
{
    using UrlShortener.Core.UnitTests.Services.TestData;

    public sealed class SnowflakeIdGeneratorTests
    {
        private readonly SnowflakeIdGeneratorSteps _steps = new SnowflakeIdGeneratorSteps();

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] 
            { 
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 0,
                    Expectation = 135168L,
                    Description = "Generating the first unique ID on January 1, 2025, at Data Center 1 with Worker 1"
                }
            },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void GenerateId_ShouldReturnUniqueId(IdUniquenessTestData scenario)
        {
            _steps
                .Given_A_TimeProvider_With(scenario.DataCenterId, scenario.WorkerId)
                .Given_The_Time_Is(scenario.GeneratedAt)
                .Given_There_Are_Id_Generated_At_The_Same_Milliseconds(scenario.GeneratedIdCount)
                .When_Generating_An_Id()
                .Then_The_Id_Should_Be(scenario.Expectation)
                .Then_The_Id_Should_Be_Unique()
                .Then_The_Id_Should_Be_TimeBased();
        }
    }
}
