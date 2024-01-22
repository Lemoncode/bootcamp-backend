using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
    public class LibraryContext : DbContext
    {

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookFile> BookFiles { get; set; }

        public DbSet<AuditEntry> AuditEntries { get; set; }

public DbSet<Publisher> Publishers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Book).Assembly);
            modelBuilder.Entity<PhisicalLibrary>()
                .ToTable("PhisicalLibraries", t =>
                {
                    t.HasComment("Bibliotecas físicas");
                });

            var auditEntryEntity = modelBuilder.Entity<AuditEntry>();
            auditEntryEntity
                .Property(p => p.OPeration)
                .HasColumnName("OperationType");

            auditEntryEntity.Property<string>("ResearchTicketId");
            modelBuilder.Entity<Author>()
                .Property(p => p.LastName)
                .HasMaxLength(200);

            var bookEntity = modelBuilder.Entity<Book>();
            bookEntity.Property(p => p.AVerage).HasPrecision(2);
            bookEntity.Property(p => p.Title)
                .UseCollation("SQL_Latin1_General_CP1_CI_AI");
            bookEntity.HasIndex(p => p.Title)
                .IsUnique(true)
                .HasDatabaseName("ux_title");

            bookEntity.Property(p => p.AVerage).HasDefaultValue(5);
            bookEntity.Property(p => p.CreationDateUtc).HasDefaultValueSql("getutcdate()");

            var authorEntity = modelBuilder.Entity<Author>();
            var publisherEntity = modelBuilder.Entity<Publisher>();
            publisherEntity.HasData(new[]
            {
                new { PublisherId = 1, Name = "Libros malos" }
            });
            base.OnModelCreating(modelBuilder);
        }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

    }
}
