using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.DataCrawling
{
    /// <summary>
    /// Responsible for crawling web-pages
    /// </summary>
    public interface ICrawlingService
    {
        Task<Dictionary<string, int>> CrawlAsync(CrawlingOptions options, CancellationToken cancellationToken);
    }
}
