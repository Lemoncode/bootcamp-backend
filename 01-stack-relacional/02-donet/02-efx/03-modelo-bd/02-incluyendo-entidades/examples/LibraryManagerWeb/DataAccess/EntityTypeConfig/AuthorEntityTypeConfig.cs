using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagerWeb.DataAccess.EntityConfig
{
    public class AuthorEntityTypeConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData(new[]
            { new { AuthorId = 1,  Name = "Juanjo", LastName = "Montiel" }
            });
            builder.Property(p => p.DisplayName).HasComputedColumnSql("Name + ' ' + LastName", true);
        }
    }
}
