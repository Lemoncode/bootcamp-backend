using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddedPrecisionToAVerage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AVerage",
                table: "Books",
                type: "decimal(2,2)",
                precision: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AVerage",
                table: "Books");
        }
    }
}
