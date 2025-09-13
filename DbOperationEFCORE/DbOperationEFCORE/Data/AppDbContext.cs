using Microsoft.EntityFrameworkCore;

namespace DbOperationEFCORE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Title = "USD", Description = "US Dollar" },
                new Currency { Id = 2, Title = "EUR", Description = "Euro" },
                new Currency { Id = 3, Title = "GBP", Description = "British Pound" },
                new Currency { Id = 4, Title = "INR", Description = "Indian" }
            );
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, Title = "EN", Description = "English" },
                new Language { Id = 2, Title = "FR", Description = "French" },
                new Language { Id = 3, Title = "ES", Description = "Spanish" },
                new Language { Id = 4, Title = "IND", Description = "Hindi" }
            );
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<BookPrice> BookPrices { get; set; }

        public DbSet<Author> Authors { get; set; }
    }
}
