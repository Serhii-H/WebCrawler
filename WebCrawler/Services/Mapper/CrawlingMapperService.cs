using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Data.Models;
using WebCrawler.Models;

namespace WebCrawler.Services.Mapper
{
    public class CrawlingMapperService : ICrawlingMapperService
    {
        public CrawlingModel Map(Crawling crawling)
        {
            if (crawling == null)
                throw new ArgumentNullException(nameof(crawling), "Unable to map Crawling");

            return new CrawlingModel
            {
                Id = crawling.Id,
                CreatedOn = crawling.CreatedOn,
                Expression = crawling.Expression,
                Status = crawling.Status.ToString(),
                Url = crawling.Url,
                HitsCount = crawling.CrawlingDetails?.Sum(detail => detail.HitsCount) ?? 0
            };
        }

        public IEnumerable<CrawlingModel> Map(IEnumerable<Crawling> crawlings)
        {
            if (crawlings == null)
                throw new ArgumentNullException(nameof(crawlings), "Unable to map Crawlings");

            return crawlings
                .Select(crawling => Map(crawling))
                .ToList();
        }

        public ICollection<CrawlingDetails> MapDetails(int crawlingId, Dictionary<string, int> crawlingResult)
        {
            if (crawlingId <= 0)
                throw new ArgumentException($"Invalid crawling Id {crawlingId}", nameof(crawlingId));

            if (crawlingResult == null)
                throw new ArgumentNullException(nameof(crawlingResult), "Unable to map crawling results");

            return crawlingResult
                .Where(result => result.Value > 0)
                .Select(result => new CrawlingDetails
                {
                    Url = result.Key,
                    HitsCount = result.Value,
                    CrawlingId = crawlingId
                }).ToList();
        }

        public IEnumerable<CrawlingDetailsModel> MapDetails(Crawling crawling)
        {
            if (crawling == null)
                throw new ArgumentNullException(nameof(crawling), "Unable to map crawling details");

            if (crawling.CrawlingDetails == null)
                return Enumerable.Empty<CrawlingDetailsModel>();

            return crawling
                .CrawlingDetails
                .Select(details => new CrawlingDetailsModel { Url = details.Url, HitsCount = details.HitsCount })
                .ToList();
        }
    }
}
