using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Data;
using WebCrawler.Models;
using WebCrawler.Services.Mapper;

namespace WebCrawler.Controllers
{
    public class CrawlingDetailsController : BaseCrawlingController
    {
        public CrawlingDetailsController(
            ICrawlingRepository repository,
            ICrawlingMapperService mapper,
            IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
        {
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<CrawlingDetailsModel>> Get(int id)
        {
            var crawling = await _repository.GetByIdAsync(id, _userId);
            
            return _mapper.MapDetails(crawling)
                .OrderBy(details => details.Url)
                .ThenBy(details => details.HitsCount);
        }
    }
}
