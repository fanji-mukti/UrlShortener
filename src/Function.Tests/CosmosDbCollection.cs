namespace Function.Tests
{
    using Xunit;

    [CollectionDefinition(CosmosDbCollection.Name)]
    public sealed class CosmosDbCollection : ICollectionFixture<TestFixture>
    {
        public const string Name = "CosmosDbCollection";
        // This class is used to apply the TestFixture to all tests in the assembly.
        // It ensures that the TestFixture is created once per test collection.
        // No additional code is needed here, as the TestFixture handles the setup and teardown.
    }
}
