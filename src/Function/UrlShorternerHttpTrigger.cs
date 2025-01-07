namespace Function
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;
    using UrlShortener.Function.DTOs;
    using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

    public class UrlShorternerHttpTrigger
    {
        private readonly ILogger _logger;

        public UrlShorternerHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UrlShorternerHttpTrigger>();
        }

        [Function("Function1")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [FromBody] ShortenUrlRequest shortenUrlRequest)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(new ShortenedUrlResponse { OriginalUrl = shortenUrlRequest.OriginalUrl, CreatedAt = DateTime.UtcNow });
        }
    }
}
