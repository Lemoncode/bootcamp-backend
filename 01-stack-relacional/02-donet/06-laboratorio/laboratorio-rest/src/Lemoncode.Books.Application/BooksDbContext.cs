using Lemoncode.Books.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lemoncode.Books.Application
{
    public class BooksDbContext
        : DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthorEntity> Authors { get; set; } = null!;
        public DbSet<BookEntity> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
