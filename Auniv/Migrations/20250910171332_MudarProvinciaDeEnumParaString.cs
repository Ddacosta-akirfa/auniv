using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auniv.Migrations
{
    /// <inheritdoc />
    public partial class MudarProvinciaDeEnumParaString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnoFundacao",
                table: "Universidades",
                newName: "DataFundacao");

            migrationBuilder.AlterColumn<string>(
                name: "SiteOficial",
                table: "Universidades",
                type: "varchar(75)",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Sigla",
                table: "Universidades",
                type: "varchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataFundacao",
                table: "Universidades",
                newName: "AnoFundacao");

            migrationBuilder.AlterColumn<string>(
                name: "SiteOficial",
                table: "Universidades",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(75)",
                oldMaxLength: 75)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Sigla",
                table: "Universidades",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldMaxLength: 15)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
