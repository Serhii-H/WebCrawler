using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using WebCrawler.Data.Models;

namespace WebCrawler.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Crawling> Crawlings { get; set; }

        public DbSet<CrawlingDetails> CrawlingDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Crawling>().Property(b => b.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            builder.Entity<Crawling>().Property(b => b.UpdatedOn).HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<Crawling>().Property(b => b.UpdatedOn).ValueGeneratedOnAddOrUpdate().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
        }
    }
}
