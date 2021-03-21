using FluentAssertions;
using NUnit.Framework;
using System;
using WebCrawler.Models;

namespace WebCrawler.Tests.Models
{
    public class CrawlOptionsTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Ctor_ExpressionIsNullOrEmpty_ThrowsArgumentNullException(string expression)
        {
            Assert.Throws<ArgumentNullException>(() => new CrawlingOptions(expression, "http://www.someurl.com"));
        }

        [Test]
        public void Ctor_ExpressionIsPassed_TrimsExpression()
        {
            var options = new CrawlingOptions(" some test expression ", "http://www.someurl.com");

            options.Expression.Should().Be("some test expression");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("www.test.com")]
        public void Ctor_UrlIsInvalid_ThrowsArgumentException(string url)
        {
            Assert.Throws<ArgumentException>(() => new CrawlingOptions("test expression", url));
        }

        [TestCase("http://url.com?q=1", "http://url.com")]
        [TestCase("http://url.com?param1=1&param2=2", "http://url.com")]
        [TestCase("http://url.com/?q=1", "http://url.com/")]
        [TestCase("http://url.com/?param1=1&param2=2", "http://url.com/")]
        [TestCase("http://url.com/childpage?q=1", "http://url.com/childpage")]
        [TestCase("http://url.com/childpage?param1=1&param2=2", "http://url.com/childpage")]
        [TestCase("http://url.com/childpage/?q=1", "http://url.com/childpage/")]
        [TestCase("http://url.com/childpage/?param1=1&param2=2", "http://url.com/childpage/")]
        [TestCase("http://url.com/childpage/", "http://url.com/childpage/")]
        public void Ctor_UrlContainsQueryParameters_SetRawBaseUrlWithoutParametersValueWithoutQueryParameters(
            string url,
            string exoectedResult)
        {
            var options = new CrawlingOptions("expression", url);

            options.BaseUrlWithoutParameters.Should().Be(exoectedResult);
        }

        [Test]
        public void Ctor_ExpressionAndUrlAreValid_SuccessfullySetAllTheProperties()
        {
            var options = new CrawlingOptions("expression", "http://url.com/testpage.html");

            options.Expression.Should().Be("expression");
            options.BaseUri.ToString().Should().Be("http://url.com/testpage.html");
            options.DomainUri.ToString().Should().Be("http://url.com/");
        }
    }
}
