using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Data;
using WebCrawler.Data.Models;
using WebCrawler.Models;
using WebCrawler.Services.DataCrawling;
using WebCrawler.Services.Mapper;

namespace WebCrawler.HostedServices
{
    /// <summary>
    /// Background service to process incoming Crawling requests which are stored in DB.
    /// </summary>
    public class CrawlingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ICrawlingService _crawlingService;
        private readonly ICrawlingMapperService _mapper;
        private readonly ILogger _logger;

        private readonly int _timeout;

        public CrawlingBackgroundService(
            IServiceProvider services,
            ICrawlingService crawlingService,
            ICrawlingMapperService mapper,
            ILogger<CrawlingBackgroundService> logger,
            int timeout = 5)
        {
            _services = services;
            _crawlingService = crawlingService;
            _mapper = mapper;
            _logger = logger;
            _timeout = timeout;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ICrawlingRepository>();

                while (true)
                {
                    var crawling = await repository.GetScheduledCrawlingAsync();

                    if (crawling != null)
                        await DoCrawling(crawling, repository, stoppingToken);

                    if (stoppingToken.IsCancellationRequested)
                        break;

                    await Task.Delay(_timeout * 1000);
                }
            }
        }

        private async Task DoCrawling(
            Crawling crawling,
            ICrawlingRepository repository,
            CancellationToken stoppingToken)
        {
            try
            {
                crawling.Status = CrawlingStatus.InProgress;
                await repository.UpdateCrawlingAsync(crawling);

                var options = new CrawlingOptions(crawling.Expression, crawling.Url);
                var crawlingResult = await _crawlingService.CrawlAsync(options, stoppingToken);

                crawling.CrawlingDetails = _mapper.MapDetails(crawling.Id, crawlingResult);
                crawling.Status = CrawlingStatus.Completed;
                await repository.UpdateCrawlingAsync(crawling);
            }
            catch (Exception e)
            {
                crawling.Status = CrawlingStatus.Failed;
                crawling.StatusText = e.ToString();
                await repository.UpdateCrawlingAsync(crawling);

                _logger.LogError(e, $"Error while crawling {crawling?.Url} in background service.");
            }
        }
    }
}
