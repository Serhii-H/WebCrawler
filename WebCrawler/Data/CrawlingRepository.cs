using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Data.Models;

namespace WebCrawler.Data
{
    public class CrawlingRepository : ICrawlingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CrawlingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Crawling> GetByIdAsync(int id, string userId)
        {
            return await _dbContext
                .Crawlings
                .Include(crawling => crawling.CrawlingDetails)
                .Where(crawling => crawling.Id == id && crawling.ApplicationUserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Crawling>> ListAsync(string userId)
        {
            return await _dbContext
                .Crawlings
                .Include(crawling => crawling.CrawlingDetails)
                .Where(crawling => crawling.ApplicationUserId == userId)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(string expression, string url, string userId)
        {
            var crawling = new Crawling
            {
                Expression = expression,
                Url = url,
                Status = CrawlingStatus.Scheduled,
                ApplicationUserId = userId
            };

            await _dbContext.Crawlings.AddAsync(crawling);
            await _dbContext.SaveChangesAsync();

            return crawling.Id;
        }

        public async Task<Crawling> GetScheduledCrawlingAsync()
        {
            return await _dbContext
                .Crawlings
                .Where(crawling => crawling.Status == CrawlingStatus.Scheduled)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateCrawlingAsync(Crawling crawling)
        {
            crawling.UpdatedOn = DateTime.UtcNow;
            _dbContext.Crawlings.Update(crawling);

            await _dbContext.SaveChangesAsync();
        }
    }
}
