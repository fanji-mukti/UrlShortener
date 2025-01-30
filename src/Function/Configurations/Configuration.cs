namespace UrlShortener.Function.Configurations
{
    internal static class Configuration
    {
        public static string GetValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

        public static int GetIntValue(string key)
        {
            return int.Parse(GetValue(key));
        }

        public static CosmosDbConfiguration GetCosmosDbConfiguration()
        {
            return new CosmosDbConfiguration
            {
                ConnectionString = GetValue("CosmosDb.ConnectionString"),
                ConnectionMode = GetValue("CosmosDb.ConnectionMode"),
            };
        }

        public static UrlShortenerServiceConfiguration GetUrlShortenerServiceConfiguration()
        {
            return new UrlShortenerServiceConfiguration
            {
                BaseUrl = GetValue("UrlShortener.BaseUrl"),
                DataCenterId = GetIntValue("UrlShortener.DataCenterId"),
                WorkerId = GetIntValue("UrlShortener.WorkerId"),
            };
        }
    }
}
