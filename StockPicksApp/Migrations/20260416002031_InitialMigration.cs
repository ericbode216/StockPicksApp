using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockPicksApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PickReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickReasons", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StockPicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StockTicker = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StockBuyDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StockBuyPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    IndexTicker = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IndexBuyPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    StockCurrentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StockCurrentPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    IndexCurrentPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    StockTotalPercentGain = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    IndexTotalPercentGain = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    StockAnnualPercentGain = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    IndexAnnualPercentGain = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPicks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "PickReasons",
                columns: new[] { "Id", "Reason", "StockId" },
                values: new object[,]
                {
                    { 1, "Microsoft is tied well into enterprises", 1 },
                    { 2, "Coupang is the amazon of South Korea", 3 }
                });

            migrationBuilder.InsertData(
                table: "StockPicks",
                columns: new[] { "Id", "IndexAnnualPercentGain", "IndexBuyPrice", "IndexCurrentPrice", "IndexTicker", "IndexTotalPercentGain", "StockAnnualPercentGain", "StockBuyDate", "StockBuyPrice", "StockCurrentDate", "StockCurrentPrice", "StockTicker", "StockTotalPercentGain" },
                values: new object[,]
                {
                    { 1, null, 1000.05m, 1005.06m, "VOO", null, null, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 400.03m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 410.10m, "MSFT", null },
                    { 2, null, 1000.05m, 1005.06m, "VOO", null, null, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 250.03m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 251.10m, "AAPL", null },
                    { 3, null, 1000.05m, 1005.06m, "VOO", null, null, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 19m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18.5m, "CPNG", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickReasons");

            migrationBuilder.DropTable(
                name: "StockPicks");
        }
    }
}
