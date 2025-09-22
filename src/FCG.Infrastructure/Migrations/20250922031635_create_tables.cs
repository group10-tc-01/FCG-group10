using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class create_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Promotions_PromotionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_CreatedAt",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_IsActive",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_Balance",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatedAt",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email_Unique",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IsActive",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PromotionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Role",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Promotion_CreatedAt",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotion_IsActive",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_Active_DateRange",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_DateRange",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_Game_DateRange",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_LibraryGame_CreatedAt",
                table: "LibraryGames");

            migrationBuilder.DropIndex(
                name: "IX_LibraryGame_IsActive",
                table: "LibraryGames");

            migrationBuilder.DropIndex(
                name: "IX_LibraryGames_Game_Sales",
                table: "LibraryGames");

            migrationBuilder.DropIndex(
                name: "IX_LibraryGames_Library_PurchaseDate",
                table: "LibraryGames");

            migrationBuilder.DropIndex(
                name: "IX_LibraryGames_PurchaseDate",
                table: "LibraryGames");

            migrationBuilder.DropIndex(
                name: "IX_Library_CreatedAt",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Library_IsActive",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Game_CreatedAt",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Game_IsActive",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Category",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Category_Price_Active",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Name",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Name_Category",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Price",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Example_CreatedAt",
                table: "Examples");

            migrationBuilder.DropIndex(
                name: "IX_Example_IsActive",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_UserId_Unique",
                table: "Wallets",
                newName: "IX_Wallets_UserId");

            migrationBuilder.RenameColumn(
                name: "DiscountValue",
                table: "Promotions",
                newName: "Discount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Wallets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LibraryGames",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Libraries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Games",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Examples",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_GameId",
                table: "Promotions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGames_GameId",
                table: "LibraryGames",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Promotions_GameId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_LibraryGames_GameId",
                table: "LibraryGames");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                newName: "IX_Wallets_UserId_Unique");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Promotions",
                newName: "DiscountValue");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Wallets",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<Guid>(
                name: "PromotionId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LibraryGames",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Libraries",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Examples",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CreatedAt",
                table: "Wallets",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_IsActive",
                table: "Wallets",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Balance",
                table: "Wallets",
                column: "Balance");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedAt",
                table: "Users",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Unique",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive",
                table: "Users",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PromotionId",
                table: "Users",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                table: "Users",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_CreatedAt",
                table: "Promotions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_IsActive",
                table: "Promotions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Active_DateRange",
                table: "Promotions",
                columns: new[] { "IsActive", "StartDate", "EndDate" },
                filter: "IsActive = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_DateRange",
                table: "Promotions",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Game_DateRange",
                table: "Promotions",
                columns: new[] { "GameId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGame_CreatedAt",
                table: "LibraryGames",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGame_IsActive",
                table: "LibraryGames",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGames_Game_Sales",
                table: "LibraryGames",
                columns: new[] { "GameId", "PurchaseDate", "PurchasePrice" });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGames_Library_PurchaseDate",
                table: "LibraryGames",
                columns: new[] { "LibraryId", "PurchaseDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryGames_PurchaseDate",
                table: "LibraryGames",
                column: "PurchaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Library_CreatedAt",
                table: "Libraries",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Library_IsActive",
                table: "Libraries",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CreatedAt",
                table: "Games",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Game_IsActive",
                table: "Games",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Category",
                table: "Games",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Category_Price_Active",
                table: "Games",
                columns: new[] { "Category", "Price", "IsActive" },
                filter: "IsActive = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                table: "Games",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Name_Category",
                table: "Games",
                columns: new[] { "Name", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_Price",
                table: "Games",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Example_CreatedAt",
                table: "Examples",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Example_IsActive",
                table: "Examples",
                column: "IsActive");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Promotions_PromotionId",
                table: "Users",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
