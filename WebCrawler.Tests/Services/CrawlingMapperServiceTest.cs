using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Data.Models;
using WebCrawler.Models;
using WebCrawler.Services.Mapper;

namespace WebCrawler.Tests.Services
{
    public class CrawlingMapperServiceTest
    {
        private CrawlingMapperService _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CrawlingMapperService();
        }

        [Test]
        public void Map_CrawlingIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Map((Crawling)null));
        }

        [Test]
        public void Map_CrawlingIsPassed_ReturnsMappedModel()
        {
            var expectedResult = new CrawlingModel
            {
                Id = 200,
                CreatedOn = new DateTime(2021, 3, 21, 1, 2, 3),
                Expression = "test expression",
                HitsCount = 21,
                Status = "InProgress",
                Url = "test url"
            };

            var input = new Crawling
            {
                Id = 200,
                CreatedOn = new DateTime(2021, 3, 21, 1, 2, 3),
                Expression = "test expression",
                Status = CrawlingStatus.InProgress,
                Url = "test url",
                CrawlingDetails = new List<CrawlingDetails>
                {
                    new CrawlingDetails { HitsCount = 10 },
                    new CrawlingDetails { HitsCount = 11 }
                }
            };

            var actualResult = _sut.Map(input);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Map_CrawlingListIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Map((IEnumerable<Crawling>)null));
        }

        [Test]
        public void Map_CrawlingListIsPassed_ReturnsMappedModel()
        {
            var expectedResult = new List<CrawlingModel>
            {
                new CrawlingModel
                {
                    Id = 1,
                    CreatedOn = new DateTime(2021, 3, 21, 1, 2, 3),
                    Expression = "test expression 0",
                    HitsCount = 8,
                    Status = "Completed",
                    Url = "test url 0"
                },
                new CrawlingModel
                {
                    Id = 2,
                    CreatedOn = new DateTime(2021, 3, 4, 4, 5, 6),
                    Expression = "test expression 1",
                    HitsCount = 17,
                    Status = "Failed",
                    Url = "test url 1"
                }
            };

            var input = new List<Crawling>
            {
                new Crawling
                {
                    Id = 1,
                    CreatedOn = new DateTime(2021, 3, 21, 1, 2, 3),
                    Expression = "test expression 0",
                    Status = CrawlingStatus.Completed,
                    Url = "test url 0",
                    CrawlingDetails = new List<CrawlingDetails>
                    {
                        new CrawlingDetails { HitsCount = 8 }
                    }
                },
                new Crawling
                {
                    Id = 2,
                    CreatedOn = new DateTime(2021, 3, 4, 4, 5, 6),
                    Expression = "test expression 1",
                    Status = CrawlingStatus.Failed,
                    Url = "test url 1",
                    CrawlingDetails = new List<CrawlingDetails>
                    {
                        new CrawlingDetails { HitsCount = 2 },
                        new CrawlingDetails { HitsCount = 15 }
                    }
                }
            };

            var actualResult = _sut.Map(input);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void MapDetails_InvalidCrawlingIdPassed_ThrowsArgumentException(int crawlingId)
        {
            Assert.Throws<ArgumentException>(() => _sut.MapDetails(crawlingId, new Dictionary<string, int>()));
        }

        [Test]
        public void MapDetails_CrawlingResultIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.MapDetails(1, null));
        }

        [Test]
        public void MapDetails_CrawlingIdAndResultArePassed_ReturnsMappedDetails()
        {
            var expectedResult = new List<CrawlingDetails>
            {
                new CrawlingDetails
                {
                    CrawlingId = 1,
                    HitsCount = 5,
                    Id = default(int),
                    Url = "www.site1.com"
                },
                new CrawlingDetails
                {
                    CrawlingId = 1,
                    HitsCount = 10,
                    Id = default(int),
                    Url = "www.site3.com"
                }
            };

            var crawlingResult = new Dictionary<string, int>
            {
                { "www.site1.com", 5 },
                { "www.site2.com", 0 },
                { "www.site3.com", 10 }
            };

            var actualResult = _sut.MapDetails(1, crawlingResult);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void MapDetails_CrawlingIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.MapDetails(null));
        }

        [Test]
        public void MapDetails_CrawlingDetailsIsNull_ReturnsEmptyCollection()
        {
            var expectedResult = Enumerable.Empty<CrawlingDetailsModel>();

            var actualResult = _sut.MapDetails(new Crawling { CrawlingDetails = null });

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void MapDetails_CrawlingDetailsContainData_ReturnsMappedCollection()
        {
            var expectedResult = new List<CrawlingDetailsModel>
            {
                new CrawlingDetailsModel{ Url = "test1.com", HitsCount = 10 },
                new CrawlingDetailsModel{ Url = "test2.com", HitsCount = 20 }
            };

            var crawling = new Crawling
            {
                CrawlingDetails = new List<CrawlingDetails>
                {
                    new CrawlingDetails { Url = "test1.com", HitsCount = 10 },
                    new CrawlingDetails { Url = "test2.com", HitsCount = 20 }
                }
            };

            var actualResult = _sut.MapDetails(crawling);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
