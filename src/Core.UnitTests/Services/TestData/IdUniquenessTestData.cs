namespace UrlShortener.Core.UnitTests.Services.TestData
{
    public sealed class IdUniquenessTestData
    {
        public long DataCenterId { get; set; }

        public long WorkerId { get; set; }

        public int GeneratedIdCount { get; set; }

        public long Expectation { get; set; }

        public DateTime GeneratedAt { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
