using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.DataScraping;

namespace WebCrawler.Tests.Services
{
    public class PageLoaderServiceTests
    {
        private TestHttpClientHandler _testHttpHandler;
        private HttpClient _fakeHttpClient;
        private PageLoaderService _sut;

        [SetUp]
        public void SetUp()
        {
            var optionsMock = new Mock<IOptions<ApplicationSettings>>(MockBehavior.Strict);
            optionsMock
                .Setup(x => x.Value)
                .Returns(new ApplicationSettings
                {
                    MediaTypeToCrawl = new[] { "text/html" }
                });

            _testHttpHandler = new TestHttpClientHandler();
            _fakeHttpClient = new HttpClient(_testHttpHandler);
            _sut = new PageLoaderService(optionsMock.Object, _fakeHttpClient);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void LoadPageContent_UrlIsNullOrEmpty_ThrowsArgumentNullException(string url)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.LoadPageContentAsync(url));
        }

        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadRequest)]
        public async Task LoadPageContent_ResponseWithoutSuccessStatusCode_ReturnsNull(HttpStatusCode httpStatusCode)
        {
            _testHttpHandler.SetHttpResponseMessage(new HttpResponseMessage(httpStatusCode));
            var expectedResult = await _sut.LoadPageContentAsync("http://someurl.com");

            expectedResult.Should().BeNull();
        }

        [Test]
        public async Task LoadPageContent_ResponseContainsUnsupportedMediaType_ReturnsNull()
        {
            _testHttpHandler.SetHttpResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test", Encoding.UTF8, "text/json")
            });
            var expectedResult = await _sut.LoadPageContentAsync("http://someurl.com");

            expectedResult.Should().BeNull();
        }

        [Test]
        public async Task LoadPageContent_ResponseContainsSupportedMediaType_ReturnsContent()
        {
            _testHttpHandler.SetHttpResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test content", Encoding.UTF8, "text/html")
            });
            var expectedResult = await _sut.LoadPageContentAsync("http://someurl.com");

            expectedResult.Should().Be("test content");
        }

        private class TestHttpClientHandler : HttpMessageHandler
        {
            private HttpResponseMessage _httpResponseMessage;

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return _httpResponseMessage;
            }

            public void SetHttpResponseMessage(HttpResponseMessage httpResponseMessage)
            {
                _httpResponseMessage = httpResponseMessage;
            }
        }
    }
}
