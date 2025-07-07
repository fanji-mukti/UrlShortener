namespace Function.Tests
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker.Http;
    using Moq;
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
            _result = await _function.ShortenUrlAsync(Mock.Of<HttpRequestData>(), shortenUrlRequest);
        }

        public async Task WhenShortUrlAsyncIsCalled(string shortUrl)
        {
            var req = new Mock<HttpRequestData>(MockBehavior.Strict, null).Object;
            _result = await _function.ShortUrlAsync(Mock.Of<HttpRequestData>(), shortUrl);
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
