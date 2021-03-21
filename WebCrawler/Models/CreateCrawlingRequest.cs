using System.ComponentModel.DataAnnotations;

namespace WebCrawler.Models
{
    public class CreateCrawlingRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Expression { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; }
    }
}
