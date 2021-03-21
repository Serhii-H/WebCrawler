using System;

namespace WebCrawler.Services.Validation
{
    public class UrlValidationService : IUrlValidationService
    {
        public bool Validate(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
