namespace Function
{
    using AutoMapper;
    using EnsureThat;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using System.ComponentModel.DataAnnotations;
    using UrlShortener.Core.Services;
    using UrlShortener.Function.DTOs;
    using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

    /// <summary>
    /// Azure Function HTTP trigger for URL shortening operations.
    /// </summary>
    public sealed class UrlShorternerHttpTrigger
    {
        private readonly IUrlShortenerService _urlShortenerService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlShorternerHttpTrigger"/> class.
        /// </summary>
        /// <param name="urlShortenerService">The URL shortener service.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UrlShorternerHttpTrigger(IUrlShortenerService urlShortenerService, IMapper mapper)
        {
            _urlShortenerService = EnsureArg.IsNotNull(urlShortenerService, nameof(urlShortenerService));
            _mapper = EnsureArg.IsNotNull(mapper, nameof(mapper));
        }

        /// <summary>
        /// Handles the HTTP POST request to shorten a URL.
        /// </summary>
        /// <param name="req">The HTTP request data.</param>
        /// <param name="shortenUrlRequest">The request body containing the URL to be shortened.</param>
        /// <returns>An <see cref="IActionResult"/> containing the shortened URL response.</returns>
        [Function("ShortenUrlV1")]
        public async Task<IActionResult> ShortenUrlAsync(
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

            var response = _mapper.Map<ShortenedUrlResponse>(shortenedUrl);

            return new OkObjectResult(response);

            bool TryValidateModel(object model, out List<ValidationResult> results)
            {
                var context = new ValidationContext(model, serviceProvider: null, items: null);
                results = new List<ValidationResult>();
                return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            }
        }

        /// <summary>
        /// Handles the HTTP GET request to retrieve the original URL from a shortened URL.
        /// </summary>
        /// <param name="req">The HTTP request data.</param>
        /// <param name="shortUrl">The shortened URL.</param>
        /// <returns>An <see cref="IActionResult"/> that redirects to the original URL or a 404 Not Found result.</returns>
        [Function("UrlRedirect")]
        public async Task<IActionResult> ShortUrlAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{shortUrl}")] HttpRequestData req,
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
