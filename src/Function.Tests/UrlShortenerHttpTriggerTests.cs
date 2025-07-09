namespace Function.Tests
{
    using Autofac;
    using Xunit;

    [Collection(CosmosDbCollection.Name)]
    public sealed class UrlShortenerHttpTriggerTests
    {
        private readonly UrlShorternerHttpTriggerSteps steps;

        public UrlShortenerHttpTriggerTests(TestFixture fixture)
        {
            this.steps = fixture.Container.Resolve<UrlShorternerHttpTriggerSteps>();
        }

        [Fact]
        public void Test()
        {
            var a = this.steps;
        }
    }
}
