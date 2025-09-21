using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Examples",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Examples",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Examples",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Libraries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0.00m),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.CheckConstraint("CK_Wallets_Balance_NonNegative", "Balance >= 0");
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryGames",
                columns: table => new
                {
                    LibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryGames", x => new { x.LibraryId, x.GameId });
                    table.ForeignKey(
                        name: "FK_LibraryGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LibraryGames_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Example_CreatedAt",
                table: "Examples",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Example_IsActive",
                table: "Examples",
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
                name: "IX_Library_CreatedAt",
                table: "Libraries",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Library_IsActive",
                table: "Libraries",
                column: "IsActive");

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
                name: "IX_Wallets_UserId_Unique",
                table: "Wallets",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryGames");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Example_CreatedAt",
                table: "Examples");

            migrationBuilder.DropIndex(
                name: "IX_Example_IsActive",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Examples");
        }
    }
}
