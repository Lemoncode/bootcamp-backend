using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class RenamedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OPeration",
                table: "AuditEntries",
                newName: "OperationType");

            migrationBuilder.CreateTable(
                name: "BookFormat",
                columns: table => new
                {
                    BookformatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookFormat", x => x.BookformatId);
                });

            migrationBuilder.CreateTable(
                name: "BookFiles",
                columns: table => new
                {
                    BookFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormatBookformatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookFiles", x => x.BookFileId);
                    table.ForeignKey(
                        name: "FK_BookFiles_BookFormat_FormatBookformatId",
                        column: x => x.FormatBookformatId,
                        principalTable: "BookFormat",
                        principalColumn: "BookformatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookFiles_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookFiles_BookId",
                table: "BookFiles",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookFiles_FormatBookformatId",
                table: "BookFiles",
                column: "FormatBookformatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookFiles");

            migrationBuilder.DropTable(
                name: "BookFormat");

            migrationBuilder.RenameColumn(
                name: "OperationType",
                table: "AuditEntries",
                newName: "OPeration");
        }
    }
}
