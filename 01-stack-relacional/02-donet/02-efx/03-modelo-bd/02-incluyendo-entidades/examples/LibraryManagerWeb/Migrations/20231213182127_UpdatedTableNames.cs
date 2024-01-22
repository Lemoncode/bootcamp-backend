using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditEntries_Country_CountryId",
                table: "AuditEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_PhisicalLibrary_Country_CountryId",
                table: "PhisicalLibrary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhisicalLibrary",
                table: "PhisicalLibrary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.RenameTable(
                name: "PhisicalLibrary",
                newName: "PhisicalLibraries");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Countries");

            migrationBuilder.RenameIndex(
                name: "IX_PhisicalLibrary_CountryId",
                table: "PhisicalLibraries",
                newName: "IX_PhisicalLibraries_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhisicalLibraries",
                table: "PhisicalLibraries",
                column: "PhisicalLibraryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditEntries_Countries_CountryId",
                table: "AuditEntries",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhisicalLibraries_Countries_CountryId",
                table: "PhisicalLibraries",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditEntries_Countries_CountryId",
                table: "AuditEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_PhisicalLibraries_Countries_CountryId",
                table: "PhisicalLibraries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhisicalLibraries",
                table: "PhisicalLibraries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "PhisicalLibraries",
                newName: "PhisicalLibrary");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Country");

            migrationBuilder.RenameIndex(
                name: "IX_PhisicalLibraries_CountryId",
                table: "PhisicalLibrary",
                newName: "IX_PhisicalLibrary_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhisicalLibrary",
                table: "PhisicalLibrary",
                column: "PhisicalLibraryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditEntries_Country_CountryId",
                table: "AuditEntries",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhisicalLibrary_Country_CountryId",
                table: "PhisicalLibrary",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
