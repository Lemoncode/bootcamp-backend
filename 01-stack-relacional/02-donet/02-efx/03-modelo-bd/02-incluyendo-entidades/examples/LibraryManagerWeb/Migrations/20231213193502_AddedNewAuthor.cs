using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "LastName", "Name" },
                values: new object[] { 1, "Montiel", "Juanjo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 1);
        }
    }
}
