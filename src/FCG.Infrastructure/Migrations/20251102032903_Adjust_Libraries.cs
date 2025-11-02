using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adjust_Libraries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryGames_Libraries_LibraryId",
                table: "LibraryGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Libraries",
                table: "Libraries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Libraries",
                table: "Libraries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_UserId",
                table: "Libraries",
                column: "UserId",
                unique: true);

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Libraries",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Libraries_UserId",
                table: "Libraries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Libraries",
                table: "Libraries",
                column: "UserId");

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
