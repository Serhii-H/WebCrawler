using System.Collections.Generic;
using WebCrawler.Data.Models;
using WebCrawler.Models;

namespace WebCrawler.Services.Mapper
{
    public interface ICrawlingMapperService
    {
        CrawlingModel Map(Crawling crawling);

        IEnumerable<CrawlingModel> Map(IEnumerable<Crawling> crawlings);

        ICollection<CrawlingDetails> MapDetails(int crawlingId, Dictionary<string, int> crawlingResult);

        IEnumerable<CrawlingDetailsModel> MapDetails(Crawling crawling);
    }
}
