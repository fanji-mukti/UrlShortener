namespace Function.Tests
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using UrlShortener.Function.DTOs;

    public class UrlShorternerHttpTriggerSteps
    {
        private readonly UrlShorternerHttpTrigger _function;
        private IActionResult _result;

        public UrlShorternerHttpTriggerSteps(
            UrlShorternerHttpTrigger trigger)
        {
            ArgumentNullException.ThrowIfNull(trigger);
            _function = trigger;
        }

        public async Task WhenShortenUrlAsyncIsCalled(ShortenUrlRequest shortenUrlRequest)
        {
            _result = await _function.ShortenUrlAsync(null, shortenUrlRequest);
        }

        public  Task WhenShortUrlAsyncIsCalledWithTheShortenedUrl()
        {
            var shortenedUrl = (ShortenedUrlResponse)((OkObjectResult)_result).Value!;

            // the response will return full qualified url.
            // Due to the test invoking the method directly instead via Http
            // we need to extract the short url from the full url.
            var shortUrl = shortenedUrl.ShortUrl.Split('/').Last();
            return this.WhenShortUrlAsyncIsCalled(shortUrl);
        }

        public async Task WhenShortUrlAsyncIsCalled(string shortUrl)
        {
            _result = await _function.ShortUrlAsync(null, shortUrl);
        }

        public UrlShorternerHttpTriggerSteps ThenResultShouldBeOkObjectResultWithExpectedResponse(ShortenedUrlResponse expected)
        {
            _result.Should().BeOfType<OkObjectResult>();
            var actualResponse = ((OkObjectResult)_result).Value;
            actualResponse.Should().BeEquivalentTo(expected);

            return this;
        }

        public UrlShorternerHttpTriggerSteps ThenResultShouldBeBadRequestObjectResult()
        {
            _result.Should().BeOfType<BadRequestObjectResult>();
            return this;
        }

        public void ThenResultShouldBeRedirectResult(string originalUrl)
        {
            _result.Should().BeOfType<RedirectResult>();
            var redirect = (RedirectResult)_result;
            redirect.Url.Should().Be(originalUrl);
        }

        public void ThenResultShouldBeNotFoundResult()
        {
            _result.Should().BeOfType<NotFoundResult>();
        }
    }
}
