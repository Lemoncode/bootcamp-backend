using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataInPublishers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publisher_PublisherId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publisher",
                table: "Publisher");

            migrationBuilder.RenameTable(
                name: "Publisher",
                newName: "Publishers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers",
                column: "PublisherId");

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PublisherId", "Name" },
                values: new object[] { 1, "Libros malos" });

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers");

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "Publishers",
                newName: "Publisher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publisher",
                table: "Publisher",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publisher_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publisher",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
