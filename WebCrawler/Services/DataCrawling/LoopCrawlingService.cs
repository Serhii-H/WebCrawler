using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Extensions;
using WebCrawler.Models;
using WebCrawler.Services.DataScraping;

namespace WebCrawler.Services.DataCrawling
{
    /// <summary>
    /// Implementation of crawling based on a loop approach.
    /// </summary>
    public class LoopCrawlingService : ICrawlingService
    {
        private readonly IHtmlScrapingService _scrapingService;
        private readonly IPageLoaderService _pageLoaderService;
        private readonly ILogger _logger;

        public LoopCrawlingService(
            IHtmlScrapingService scrapingService,
            IPageLoaderService pageLoaderService,
            ILogger<LoopCrawlingService> logger)
        {
            _scrapingService = scrapingService;
            _pageLoaderService = pageLoaderService;
            _logger = logger;
        }

        public async Task<Dictionary<string, int>> CrawlAsync(CrawlingOptions options, CancellationToken cancellationToken)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options cannot be null.");

            try
            {
                var processedUrls = new Dictionary<string, int>();

                var urlsToProcess = new Queue<string>();
                urlsToProcess.Enqueue(options.BaseUri.ToString());

                while (urlsToProcess.Count > 0)
                {
                    var url = urlsToProcess.Dequeue();

                    if (processedUrls.ContainsKey(url))
                        continue;

                    var pageContent = await _pageLoaderService.LoadPageContentAsync(url);

                    var count = _scrapingService.CountOccurrence(options.Expression, pageContent);
                    processedUrls.Add(url, count);

                    var hrefs = _scrapingService.GetRelativeHrefs(pageContent);

                    foreach (var href in hrefs)
                    {
                        var absoluteUri = new Uri(options.DomainUri, href).ToString();
                        var isSubPage = options.BaseUrlWithoutParameters.IsSubPage(absoluteUri);

                        if (isSubPage)
                            urlsToProcess.Enqueue(absoluteUri);
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }

                return processedUrls;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while crawling.");
                throw;
            }
        }
    }
}
