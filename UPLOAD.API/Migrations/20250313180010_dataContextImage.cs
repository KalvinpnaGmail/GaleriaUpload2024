using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPLOAD.API.Migrations
{
    /// <inheritdoc />
    public partial class dataContextImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CabeceraImages_CabeceraImageId1",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CabeceraImageId1",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CabeceraImageId1",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CabeceraImageId1",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_CabeceraImageId1",
                table: "Images",
                column: "CabeceraImageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_CabeceraImages_CabeceraImageId1",
                table: "Images",
                column: "CabeceraImageId1",
                principalTable: "CabeceraImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
