using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Data.Models;

namespace WebCrawler.Data
{
    public interface ICrawlingRepository
    {
        Task<Crawling> GetByIdAsync(int id, string userId);

        Task<IEnumerable<Crawling>> ListAsync(string userId);

        Task<int> CreateAsync(string expression, string url, string userId);

        Task<Crawling> GetScheduledCrawlingAsync();

        Task UpdateCrawlingAsync(Crawling crawling);
    }
}
