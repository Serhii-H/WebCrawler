namespace WebCrawler.Models
{
    public class ApplicationSettings
    {
        public string[] MediaTypeToCrawl { get; set; }

        public string[] HrefsToIgnore { get; set; }
    }
}
