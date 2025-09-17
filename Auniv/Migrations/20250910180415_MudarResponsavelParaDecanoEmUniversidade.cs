using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auniv.Migrations
{
    /// <inheritdoc />
    public partial class MudarResponsavelParaDecanoEmUniversidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Responsavel",
                table: "Universidades",
                newName: "Decano");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decano",
                table: "Universidades",
                newName: "Responsavel");
        }
    }
}
