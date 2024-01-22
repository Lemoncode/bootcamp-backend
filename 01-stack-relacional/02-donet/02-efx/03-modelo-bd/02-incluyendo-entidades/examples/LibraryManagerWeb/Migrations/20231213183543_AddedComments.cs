using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddedComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "PhisicalLibraries",
                comment: "Bibliotecas físicas");

            migrationBuilder.AlterTable(
                name: "AuditEntries",
                comment: "Clase para incluir entradas de auditoría con respecto a las operaciones realizadas en nuestra biblioteca.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "PhisicalLibraries",
                oldComment: "Bibliotecas físicas");

            migrationBuilder.AlterTable(
                name: "AuditEntries",
                oldComment: "Clase para incluir entradas de auditoría con respecto a las operaciones realizadas en nuestra biblioteca.");
        }
    }
}
