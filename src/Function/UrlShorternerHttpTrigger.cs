namespace Function
{
    using System.Net;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;
    using UrlShortener.Function.DTOs;

    public class UrlShorternerHttpTrigger
    {
        private readonly ILogger _logger;

        public UrlShorternerHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UrlShorternerHttpTrigger>();
        }

        [Function("Function1")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [FromBody] ShortenUrlRequest shortenUrlRequest)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
