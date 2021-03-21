using System.Linq;

namespace WebCrawler.Extensions
{
    public static class UrlExtensions
    {
        /// <summary>
        /// Detects whether a given URL string is based on the base URL, so that could be considered as its subpage.
        /// </summary>
        public static bool IsSubPage(this string baseUrl, string absoluteUrl) =>
            absoluteUrl.StartsWith(baseUrl);

        /// <summary>
        /// Remove trailing slash if exists
        /// </summary>
        public static string RemoveTrailingSlash(this string url) =>
            !string.IsNullOrWhiteSpace(url) && url.EndsWith('/') ? url.TrimEnd('/') : url;

        /// <summary>
        /// Remove hash identifier if exists
        /// </summary>
        public static string RemoveHashIdentifier(this string url) =>
            !string.IsNullOrWhiteSpace(url) && url.IndexOf('#') >= 0 ? url.Split('#').First() : url;
    }
}
