using System.Threading.Tasks;

namespace WebCrawler.Services.DataScraping
{
    /// <summary>
    /// Loads page content
    /// </summary>
    public interface IPageLoaderService
    {
        Task<string> LoadPageContentAsync(string url);
    }
}
