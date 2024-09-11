using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPLOAD.API.Migrations
{
    /// <inheritdoc />
    public partial class addcabeceradeimg3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CabeceraImageId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CabeceraImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabeceraImages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CabeceraImageId",
                table: "Images",
                column: "CabeceraImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_CabeceraImages_CabeceraImageId",
                table: "Images",
                column: "CabeceraImageId",
                principalTable: "CabeceraImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CabeceraImages_CabeceraImageId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "CabeceraImages");

            migrationBuilder.DropIndex(
                name: "IX_Images_CabeceraImageId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CabeceraImageId",
                table: "Images");
        }
    }
}
