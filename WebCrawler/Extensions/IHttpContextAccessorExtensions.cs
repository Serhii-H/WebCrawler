using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebCrawler.Extensions
{
    public static class IHttpContextAccessorExtensions
    {
        public static string GetUserId(this IHttpContextAccessor contextAccessor) =>
            contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
