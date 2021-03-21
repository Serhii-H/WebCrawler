using System;

namespace WebCrawler.Data.Models
{
    public class CrawlingDetails
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int HitsCount { get; set; }

        public int CrawlingId { get; set; }
    }
}
