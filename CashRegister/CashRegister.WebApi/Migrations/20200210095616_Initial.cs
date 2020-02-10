using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CashRegister.WebApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Alexikon");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Alexikon",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                schema: "Alexikon",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptTimestamp = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptLines",
                schema: "Alexikon",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    ReceiptID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptLines", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceiptLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalSchema: "Alexikon",
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptLines_Receipts_ReceiptID",
                        column: x => x.ReceiptID,
                        principalSchema: "Alexikon",
                        principalTable: "Receipts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptLines_ProductID",
                schema: "Alexikon",
                table: "ReceiptLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptLines_ReceiptID",
                schema: "Alexikon",
                table: "ReceiptLines",
                column: "ReceiptID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptLines",
                schema: "Alexikon");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Alexikon");

            migrationBuilder.DropTable(
                name: "Receipts",
                schema: "Alexikon");
        }
    }
}
