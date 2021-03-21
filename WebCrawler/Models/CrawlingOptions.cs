using System;
using System.Linq;

namespace WebCrawler.Models
{
    public class CrawlingOptions
    {
        /// <summary>
        /// Expression to be find.
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// URI to start crawling from.
        /// </summary>
        public Uri BaseUri { get; private set; }

        /// <summary>
        /// BaseUri string without query parameters.
        /// </summary>
        public string BaseUrlWithoutParameters { get; private set; }

        /// <summary>
        /// Domain of the BaseUri.
        /// </summary>
        public Uri DomainUri { get; private set; }

        public CrawlingOptions(string expression, string url)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentNullException(nameof(expression), "Expression cannot be empty.");

            if (!Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var parsedUri))
                throw new ArgumentException($"Could not parse invalid URL: {url}.", nameof(url));

            Expression = expression.Trim();
            BaseUri = parsedUri;
            BaseUrlWithoutParameters = url.Split('?').First().Trim();

            var domainUri = BaseUri.GetLeftPart(UriPartial.Authority);
            DomainUri = new Uri(domainUri, UriKind.Absolute);
        }
    }
}
