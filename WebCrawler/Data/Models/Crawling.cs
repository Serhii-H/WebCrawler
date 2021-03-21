using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebCrawler.Data.Models
{
    public class Crawling
    {
        public int Id { get; set; }

        public CrawlingStatus Status { get; set; }

        public string StatusText { get; set; }

        [Required]
        public string Expression { get; set; }

        [Required]
        public string Url { get; set; }

        public DateTime UpdatedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ICollection<CrawlingDetails> CrawlingDetails { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
