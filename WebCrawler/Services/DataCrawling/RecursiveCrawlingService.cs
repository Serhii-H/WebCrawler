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
    /// Implementation of crawling based on a recursion approach.
    /// </summary>
    public class RecursiveCrawlingService : ICrawlingService
    {
        private readonly IHtmlScrapingService _scrapingService;
        private readonly IPageLoaderService _pageLoaderService;
        private readonly ILogger _logger;

        public RecursiveCrawlingService(
            IHtmlScrapingService scrapingService,
            IPageLoaderService pageLoaderService,
            ILogger<RecursiveCrawlingService> logger)
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
                return await CrawlRecursive(options, cancellationToken);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error while crawling.");
                throw;
            }
        }

        private async Task<Dictionary<string, int>> CrawlRecursive(
            CrawlingOptions crawlOptions,
            CancellationToken cancellationToken,
            string url = null,
            Dictionary<string, int> processedUrls = null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(url))
                url = crawlOptions.BaseUri.ToString();

            if (processedUrls == null)
                processedUrls = new Dictionary<string, int>();

            var pageContent = await _pageLoaderService.LoadPageContentAsync(url);

            var count = _scrapingService.CountOccurrence(crawlOptions.Expression, pageContent);
            processedUrls.Add(url, count);

            var hrefs = _scrapingService.GetRelativeHrefs(pageContent);

            foreach (var href in hrefs)
            {
                var absoluteUri = new Uri(crawlOptions.DomainUri, href).ToString();
                var isSubPage = crawlOptions.BaseUrlWithoutParameters.IsSubPage(absoluteUri);

                if (isSubPage && !processedUrls.ContainsKey(absoluteUri))
                    await CrawlRecursive(crawlOptions, cancellationToken, absoluteUri, processedUrls);
            }

            return processedUrls;
        }
    }
}
