using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLibraryGameFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryGames_Libraries_LibraryId",
                table: "LibraryGames");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Libraries_Id",
                table: "Libraries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryGames_Libraries_LibraryId",
                table: "LibraryGames",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryGames_Libraries_LibraryId",
                table: "LibraryGames");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Libraries_Id",
                table: "Libraries");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryGames_Libraries_LibraryId",
                table: "LibraryGames",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
