namespace Function
{
    using EnsureThat;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using System.ComponentModel.DataAnnotations;
    using UrlShortener.Core.Services;
    using UrlShortener.Function.DTOs;
    using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

    public class UrlShorternerHttpTrigger
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlShorternerHttpTrigger(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = EnsureArg.IsNotNull(urlShortenerService, nameof(urlShortenerService));
        }

        [Function("ShortenUrlV1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/data/shorten")] HttpRequestData req,
            [FromBody] ShortenUrlRequest shortenUrlRequest)
        {
            if (!TryValidateModel(shortenUrlRequest, out var validationResults))
            {
                 return new BadRequestObjectResult(validationResults);
            }

            var shortenedUrl = await _urlShortenerService
                .ShortenUrlAsync(shortenUrlRequest.OriginalUrl, shortenUrlRequest.ExpiresAt)
                .ConfigureAwait(false);

            var response = new ShortenedUrlResponse
            {
                OriginalUrl = shortenedUrl.OriginalUrl,
                ShortUrl = shortenedUrl.ShortUrl,
                CreatedAt = shortenedUrl.CreatedAt,
                ExpiresAt = shortenedUrl.ExpiresAt,
            };

            return new OkObjectResult(response);

            bool TryValidateModel(object model, out List<ValidationResult> results)
            {
                var context = new ValidationContext(model, serviceProvider: null, items: null);
                results = new List<ValidationResult>();
                return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            }
        }

        [Function("shortUrlV1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/{shortUrl}")] HttpRequestData req,
            [FromRoute] string shortUrl)
        { 
            var shortenedUrl = await _urlShortenerService
                .GetOriginalUrlAsync(shortUrl)
                .ConfigureAwait(false);

            if (shortenedUrl == null)
            { 
                return new NotFoundResult();
            }

            return new RedirectResult(shortenedUrl.OriginalUrl);
        }
    }
}
