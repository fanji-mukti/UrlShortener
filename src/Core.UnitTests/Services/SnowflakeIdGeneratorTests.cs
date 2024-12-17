namespace UrlShortener.Core.UnitTests.Services
{
    public class SnowflakeIdGeneratorTests
    {
        private readonly SnowflakeIdGeneratorSteps _steps;

        public SnowflakeIdGeneratorTests()
        {
            _steps = new SnowflakeIdGeneratorSteps();
        }

        [Fact]
        public void GenerateId_ShouldReturnUniqueId()
        {
            _steps.Given_A_TimeProvider_And_SnowflakeIdGenerator();
            _steps.When_Generating_An_Id();
            _steps.Then_The_Id_Should_Be_Unique();
        }

        [Fact]
        public void GenerateId_ShouldContainTimeBasedComponent()
        {
            _steps.Given_A_TimeProvider_And_SnowflakeIdGenerator();
            _steps.When_Generating_An_Id();
            _steps.Then_The_Id_Should_Be_TimeBased();
        }
    }
}
