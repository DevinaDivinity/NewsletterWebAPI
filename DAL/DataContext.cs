using Microsoft.EntityFrameworkCore;
using NewsletterAPI.Entities;

namespace NewsletterAPI.DAL
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<NewsletterUser> NewsletterUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NewsletterUser>();
        }
    }
}
