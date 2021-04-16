using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations.Migrations
{
    public partial class Add_Account_AuditInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Updated",
                schema: "model",
                table: "Account",
                newName: "AuditInfo_Updated");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "model",
                table: "Account",
                newName: "AuditInfo_Created");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AuditInfo_Created",
                schema: "model",
                table: "Account",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuditInfo_Updated",
                schema: "model",
                table: "Account",
                newName: "Updated");

            migrationBuilder.RenameColumn(
                name: "AuditInfo_Created",
                schema: "model",
                table: "Account",
                newName: "Created");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                schema: "model",
                table: "Account",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);
        }
    }
}
