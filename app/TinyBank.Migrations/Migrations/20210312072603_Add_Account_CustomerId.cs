using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations.Migrations
{
    public partial class Add_Account_CustomerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Customer_CustomerId",
                schema: "model",
                table: "Account");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "model",
                table: "Account",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Customer_CustomerId",
                schema: "model",
                table: "Account",
                column: "CustomerId",
                principalSchema: "model",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Customer_CustomerId",
                schema: "model",
                table: "Account");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "model",
                table: "Account",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Customer_CustomerId",
                schema: "model",
                table: "Account",
                column: "CustomerId",
                principalSchema: "model",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
