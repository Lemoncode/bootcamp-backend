using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.Reflection.Emit;

namespace LibraryManagerWeb.DataAccess.EntityTypeConfig
{
    public class BookEntityTypeConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Ignore(p => p.LoadedDate);
            builder.Property(p => p.AVerage).HasPrecision(2);
            builder.Property(p => p.Title)
                .UseCollation("SQL_Latin1_General_CP1_CI_AI");
            builder.HasIndex(p => p.Title)
                .IsUnique(true)
                .HasDatabaseName("ux_title");

            builder.Property(p => p.AVerage).HasDefaultValue(5);
            builder.Property(p => p.CreationDateUtc).HasDefaultValueSql("getutcdate()");

            builder.HasOne(p => p.Author).WithMany(p => p.Books)
                .IsRequired();
            builder.HasOne(p => p.BookImage).WithOne(p => p.Book);

            builder.HasMany(p => p.Tags).WithMany(p => p.Books);

        }
    }
}
