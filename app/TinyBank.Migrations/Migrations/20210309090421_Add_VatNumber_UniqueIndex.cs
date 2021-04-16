using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations.Migrations
{
    public partial class Add_VatNumber_UniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VatNumber",
                schema: "model",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_VatNumber",
                schema: "model",
                table: "Customer",
                column: "VatNumber",
                unique: true,
                filter: "[VatNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer_VatNumber",
                schema: "model",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "VatNumber",
                schema: "model",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
