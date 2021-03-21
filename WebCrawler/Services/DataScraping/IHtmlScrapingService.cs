using System.Collections.Generic;

namespace WebCrawler.Services.DataScraping
{
    /// <summary>
    /// Responsible for scraping the data from a particular html markup.
    /// </summary>
    public interface IHtmlScrapingService
    {
        /// <summary>
        /// Returns an array of relative hrefs from the provided html.
        /// </summary>
        IEnumerable<string> GetRelativeHrefs(string html);

        /// <summary>
        /// Returns occurrence count of a particular expression within the provided html.
        /// </summary>
        int CountOccurrence(string expression, string html);
    }
}
