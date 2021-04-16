using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations.Migrations
{
    public partial class Add_Model_Card : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Card",
                schema: "model",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CardType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "AccountCard",
                schema: "model",
                columns: table => new
                {
                    AccountsAccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardsCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCard", x => new { x.AccountsAccountId, x.CardsCardId });
                    table.ForeignKey(
                        name: "FK_AccountCard_Account_AccountsAccountId",
                        column: x => x.AccountsAccountId,
                        principalSchema: "model",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountCard_Card_CardsCardId",
                        column: x => x.CardsCardId,
                        principalSchema: "model",
                        principalTable: "Card",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCard_CardsCardId",
                schema: "model",
                table: "AccountCard",
                column: "CardsCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCard",
                schema: "model");

            migrationBuilder.DropTable(
                name: "Card",
                schema: "model");
        }
    }
}
