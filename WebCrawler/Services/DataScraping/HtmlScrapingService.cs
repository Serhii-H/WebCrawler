using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebCrawler.Extensions;
using WebCrawler.Models;

namespace WebCrawler.Services.DataScraping
{
    public class HtmlScrapingService : IHtmlScrapingService
    {
        private string[] _hrefsToIgnore;
        private Regex _hrefRegex;

        public HtmlScrapingService(IOptions<ApplicationSettings> settings)
        {
            if (settings.Value.HrefsToIgnore == null)
                throw new ArgumentNullException(nameof(settings.Value.HrefsToIgnore), $"Missing required setting: {nameof(settings.Value.HrefsToIgnore)}");

            _hrefsToIgnore = settings.Value.HrefsToIgnore;

            var regexOptions = RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;
            _hrefRegex = new Regex("(?:href|src)=[\"|']?(.*?)[\"|'|>]+", regexOptions);
        }

        public IEnumerable<string> GetRelativeHrefs(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return Array.Empty<string>();

            var matches = _hrefRegex.Matches(html);

            var hrefs = matches
                .Where(match => match.Success && match.Groups.Count >= 2)
                .Select(match => match.Groups[1].Value?.Trim()?.RemoveTrailingSlash()?.RemoveHashIdentifier())
                .Distinct(StringComparer.OrdinalIgnoreCase);

            return FilterOutUnsupportedHrefs(hrefs);
        }

        private IEnumerable<string> FilterOutUnsupportedHrefs(IEnumerable<string> hrefs)
        {
            return hrefs.Where(href =>
                // Only relative root-based links
                href.StartsWith('/') &&

                href.Length > 1 &&
                !href.StartsWith("//") &&
                !_hrefsToIgnore.Any(extension => href.Contains(extension)))
                .ToList();
        }

        public int CountOccurrence(string expression, string html)
        {
            if (string.IsNullOrWhiteSpace(expression) || string.IsNullOrWhiteSpace(html))
                return 0;

            var count = 0;
            var index = html.IndexOf(expression, StringComparison.OrdinalIgnoreCase);

            while (index >= 0)
            {
                count++;

                var startIndex = index + expression.Length;
                index = html.IndexOf(expression, startIndex, StringComparison.OrdinalIgnoreCase);
            }

            return count;
        }
    }
}
