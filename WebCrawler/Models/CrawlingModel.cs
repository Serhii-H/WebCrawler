using System;

namespace WebCrawler.Models
{
    public class CrawlingModel
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Url { get; set; }

        public string Expression { get; set; }

        public int HitsCount { get; set; }
    }
}
