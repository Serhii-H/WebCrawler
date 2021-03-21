using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.DataScraping
{
    public class PageLoaderService : IPageLoaderService
    {
        private readonly string[] _allowedMediaType;
        private readonly HttpClient _httpClient;

        public PageLoaderService(
            IOptions<ApplicationSettings> settings,
            HttpClient httpClient)
        {
            if (settings.Value.MediaTypeToCrawl == null)
                throw new ArgumentNullException(nameof(settings.Value.MediaTypeToCrawl), $"Missing required setting: {nameof(settings.Value.MediaTypeToCrawl)}");

            _allowedMediaType = settings.Value.MediaTypeToCrawl;
            _httpClient = httpClient;
        }

        public async Task<string> LoadPageContentAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url), "Url cannot be null or empty.");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            if (response.Content == null)
                return null;

            var reponseMediaType = response.Content.Headers?.ContentType?.MediaType;

            if (!_allowedMediaType.Any(mediaType => mediaType.Equals(reponseMediaType, StringComparison.OrdinalIgnoreCase)))
                return null;

            return await response.Content.ReadAsStringAsync();
        }
    }
}
