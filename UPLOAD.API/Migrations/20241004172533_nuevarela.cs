using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPLOAD.API.Migrations
{
    /// <inheritdoc />
    public partial class nuevarela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Provincias_CountryId_Name",
                table: "Provincias");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Name",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ProvinciaId_Name",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_CountryId",
                table: "Provincias",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinciaId",
                table: "Cities",
                column: "ProvinciaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Provincias_CountryId",
                table: "Provincias");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ProvinciaId",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_CountryId_Name",
                table: "Provincias",
                columns: new[] { "CountryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinciaId_Name",
                table: "Cities",
                columns: new[] { "ProvinciaId", "Name" },
                unique: true);
        }
    }
}
