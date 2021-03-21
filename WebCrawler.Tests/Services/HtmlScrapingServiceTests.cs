using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using WebCrawler.Models;
using WebCrawler.Services.DataScraping;

namespace WebCrawler.Tests.Services
{
    public class HtmlScrapingServiceTests
    {
        private string _fakeHtml;
        private HtmlScrapingService _sut;

        [SetUp]
        public void SetUp()
        {
            var optionsMock = new Mock<IOptions<ApplicationSettings>>(MockBehavior.Strict);
            optionsMock
                .Setup(x => x.Value)
                .Returns(new ApplicationSettings
                {
                    HrefsToIgnore = new[] { ".jpg" }
                });

            _sut = new HtmlScrapingService(optionsMock.Object);

            _fakeHtml = @"
                <html>
                    <head></head>
                    <body>
                        <h1>Hello World!</h1>
                        <a href='/home/'>Home page</a>
                        <a href='/home/subhome'>Subhome
                        <a href='/images/logo.jpg'></a>
                        <a href='images'>Gallery page</a>
                        <a href='./contact'>Contact</a>
                        <a href='../parent'>Main</a>
                        <a href='https://www.google.com'>Google</a>
                        <a href='/HOME'>Go to Home</a>
                        <a href='/blog#articles'>...</a>
                    </body>
                </html>";
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetRelativeHrefs_HtmlIsNullOrEmpty_ReturnsEmptyArray(string html)
        {
            var expectedResult = Array.Empty<string>();

            var actualResult = _sut.GetRelativeHrefs(html);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetRelativeHrefs_ValidHtmlIsPassed_ReturnsRelativeHrefs()
        {
            var expectedResult = new[] { "/home", "/home/subhome", "/blog" };

            var actualResult = _sut.GetRelativeHrefs(_fakeHtml);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void CountOccurrence_ExpressionIsNullOrEmpty_ReturnsZero(string expression)
        {
            var count = _sut.CountOccurrence(expression, _fakeHtml);

            count.Should().Be(0);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void CountOccurrence_HtmlIsNullOrEmpty_ReturnsZero(string html)
        {
            var count = _sut.CountOccurrence("expression", html);

            count.Should().Be(0);
        }

        [Test]
        public void CountOccurrence_ExpressionIsNotPresentedInProvidedHtml_ReturnsZero()
        {
            var count = _sut.CountOccurrence("not existing expression", _fakeHtml);

            count.Should().Be(0);
        }

        [Test]
        public void CountOccurrence_ExpressionIsPresentedInProvidedHtml_ReturnsOccurrenceCount()
        {
            var count = _sut.CountOccurrence("page", _fakeHtml);

            count.Should().Be(2);
        }
    }
}
