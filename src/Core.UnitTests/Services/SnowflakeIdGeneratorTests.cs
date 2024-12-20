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
                },
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 2,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 0,
                    Expectation = 266240L,
                    Description = "Generating the first unique ID on January 1, 2025, at Data Center 2 with Worker 1"
                },
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 2,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 0,
                    Expectation = 139264L,
                    Description = "Generating the first unique ID on January 1, 2025, at Data Center 1 with Worker 2"
                },
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 9,
                    Expectation = 135177L,
                    Description = "Generating the ninth unique ID on January 1, 2025, at Data Center 1 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 2,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 9,
                    Expectation = 266249L,
                    Description = "Generating the ninth unique ID on January 1, 2025, at Data Center 2 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 2,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 9,
                    Expectation = 139273L,
                    Description = "Generating the ninth unique ID on January 1, 2025, at Data Center 1 with Worker 2"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 0, 1, DateTimeKind.Utc),
                    GeneratedIdCount = 1,
                    Expectation = 4194439169L,
                    Description = "Generating the ninth unique ID on January 1, 2025 on different millisecond, at Data Center 1 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 1, 0, 1, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 1,
                    Expectation = 251658375169L,
                    Description = "Generating the ninth unique ID on January 1, 2025 on different second, at Data Center 1 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 1, 1, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 1,
                    Expectation = 15099494535169L,
                    Description = "Generating the ninth unique ID on January 1, 2025 on different minute, at Data Center 1 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 1,
                    Expectation = 362387865735169L,
                    Description = "Generating the ninth unique ID on January 1, 2025 on different hour, at Data Center 1 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 1,
                    Expectation = 362387865735169L,
                    Description = "Generating the ninth unique ID on January 2, 2025 on different hour, at Data Center 1 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 1,
                    Expectation = 11234023833735169L,
                    Description = "Generating the ninth unique ID on February 1, 2025 on different hour, at Data Center 1 with Worker 1"
                }
            },
            new object[]
            {
                new IdUniquenessTestData
                {
                    DataCenterId = 1,
                    WorkerId = 1,
                    GeneratedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    GeneratedIdCount = 1,
                    Expectation = 132271570944135169L,
                    Description = "Generating the ninth unique ID on Januar 1, 2026 on different hour, at Data Center 1 with Worker 1"
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
