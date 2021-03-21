using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Data;
using WebCrawler.Models;
using WebCrawler.Services.Mapper;
using WebCrawler.Services.Validation;

namespace WebCrawler.Controllers
{
    public class CrawlingController : BaseCrawlingController
    {
        private readonly IUrlValidationService _urlValidation;

        public CrawlingController(
            ICrawlingRepository repository,
            IUrlValidationService urlValidation,
            ICrawlingMapperService mapper,
            IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
        {
            _urlValidation = urlValidation;
        }

        [HttpGet("{id}")]
        public async Task<CrawlingModel> Get(int id)
        {
            var crawling = await _repository.GetByIdAsync(id, _userId);
            return _mapper.Map(crawling);
        }

        [HttpGet]
        public async Task<IEnumerable<CrawlingModel>> GetList()
        {
            var crawlings = await _repository.ListAsync(_userId);
            return _mapper.Map(crawlings);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCrawlingRequest request)
        {
            // TODO: Better to create a middleware for validation purposes rather than add such IF in each controller.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_urlValidation.Validate(request.Url))
                return BadRequest(new { errorText = $"Provided URL {request.Url} is not valid." });

            var id = await _repository.CreateAsync(request.Expression, request.Url, _userId);

            return Created(nameof(Get), id);
        }
    }
}
