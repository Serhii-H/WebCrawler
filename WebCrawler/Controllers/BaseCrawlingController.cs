using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebCrawler.Data;
using WebCrawler.Extensions;
using WebCrawler.Services.Mapper;

namespace WebCrawler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseCrawlingController : ControllerBase
    {
        protected readonly ICrawlingRepository _repository;
        protected readonly ICrawlingMapperService _mapper;
        protected readonly string _userId;

        public BaseCrawlingController(
            ICrawlingRepository repository,
            ICrawlingMapperService mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _userId = httpContextAccessor.GetUserId();

            if (string.IsNullOrWhiteSpace(_userId))
                throw new ArgumentException("Could not find User Id.", nameof(_userId));
        }
    }
}
