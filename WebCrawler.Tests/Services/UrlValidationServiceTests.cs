using FluentAssertions;
using NUnit.Framework;
using WebCrawler.Services.Validation;

namespace WebCrawler.Tests.Services
{
    public class UrlValidationServiceTests
    {
        private UrlValidationService _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new UrlValidationService();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Validate_UrlIsNullOrEmpty_ReturnsFalse(string emptyUrl)
        {
            var actualResult = _sut.Validate(emptyUrl);

            actualResult.Should().BeFalse();
        }

        [TestCase("google.com")]
        [TestCase("http:google.com")]
        [TestCase("http:\\google.com")]
        [TestCase("http\\google.com")]
        [TestCase("http")]
        [TestCase("http:")]
        [TestCase("http://")]
        [TestCase("http:\\")]
        public void Validate_InvalidUrlIsPassed_ReturnsFalse(string invalidUrl)
        {
            var actualResult = _sut.Validate(invalidUrl);

            actualResult.Should().BeFalse();
        }

        [TestCase("http://google.com")]
        [TestCase("https://google.com")]
        [TestCase("http://google.com/somepage")]
        [TestCase("http://google.com/somepage?param1=1&param2=2")]
        [TestCase("http://google.com/somepage#anchor?param1=1&param2=2")]
        public void Validate_ValidUrlIsPassed_ReturnsTrue(string url)
        {
            var actualResult = _sut.Validate(url);

            actualResult.Should().BeTrue();
        }
    }
}
