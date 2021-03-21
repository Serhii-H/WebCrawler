using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.DataCrawling;
using WebCrawler.Services.DataScraping;

namespace WebCrawler.Tests.Services
{
    public abstract class CrawlingServiceTests
    {
        protected Mock<IHtmlScrapingService> _scrapingServiceMock;
        protected Mock<IPageLoaderService> _pageLoaderServiceMock;

        private ICrawlingService _sut;

        [SetUp]
        public void SetUp()
        {
            _scrapingServiceMock = new Mock<IHtmlScrapingService>(MockBehavior.Strict);
            _pageLoaderServiceMock = new Mock<IPageLoaderService>(MockBehavior.Strict);

            _sut = CreateSut();
        }

        protected abstract ICrawlingService CreateSut();

        [Test]
        public void CrawlAsync_OptionsIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CrawlAsync(null, CancellationToken.None));
        }

        [Test]
        public async Task CrawlAsync_ValidOptionsPassed_PerformsCrawling()
        {
            // Arrange

            var expectedResult = new Dictionary<string, int>
            {
                { "http://rooturl.com/index", 1 },
                { "http://rooturl.com/index/home", 0 },
                { "http://rooturl.com/index/home/subhome", 2 }
            };

            var expression = "hello world!";
            var options = new CrawlingOptions(expression, "http://rooturl.com/index");

            // First set of Mocked data
            _pageLoaderServiceMock
                .Setup(x => x.LoadPageContentAsync("http://rooturl.com/index"))
                .Returns(Task.FromResult("root html"));

            _scrapingServiceMock
                .Setup(x => x.CountOccurrence(expression, "root html"))
                .Returns(1);

            _scrapingServiceMock
                .Setup(x => x.GetRelativeHrefs("root html"))
                .Returns(new[] { "/index/home", "/about" });


            // Second set of Mocked data
            _pageLoaderServiceMock
                .Setup(x => x.LoadPageContentAsync("http://rooturl.com/index/home"))
                .Returns(Task.FromResult("home html"));

            _scrapingServiceMock
                .Setup(x => x.CountOccurrence(expression, "home html"))
                .Returns(0);

            _scrapingServiceMock
                .Setup(x => x.GetRelativeHrefs("home html"))
                .Returns(new[] { "index/home/subhome" });


            // Third set of Mocked data

            _pageLoaderServiceMock
                .Setup(x => x.LoadPageContentAsync("http://rooturl.com/index/home/subhome"))
                .Returns(Task.FromResult("subhome html"));

            _scrapingServiceMock
                .Setup(x => x.CountOccurrence(expression, "subhome html"))
                .Returns(2);

            _scrapingServiceMock
                .Setup(x => x.GetRelativeHrefs("subhome html"))
                .Returns(new[] { "/home" });

            // Act
            var actualResult = await _sut.CrawlAsync(options, CancellationToken.None);

            // Assert

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }

    public class LoopCrawlingServiceTests : CrawlingServiceTests
    {
        protected override ICrawlingService CreateSut()
        {
            return new LoopCrawlingService(
                _scrapingServiceMock.Object,
                _pageLoaderServiceMock.Object,
                new Mock<ILogger<LoopCrawlingService>>(MockBehavior.Loose).Object);
        }
    }

    public class RecursiveCrawlingServiceTests : CrawlingServiceTests
    {
        protected override ICrawlingService CreateSut()
        {
            return new RecursiveCrawlingService(
                _scrapingServiceMock.Object,
                _pageLoaderServiceMock.Object,
                new Mock<ILogger<RecursiveCrawlingService>>(MockBehavior.Loose).Object);
        }
    }
}
