using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatChecker.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suggestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SettlerId",
                table: "Suggestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Suggestions",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_SettlerId",
                table: "Suggestions",
                column: "SettlerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Users_SettlerId",
                table: "Suggestions",
                column: "SettlerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Users_SettlerId",
                table: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_Suggestions_SettlerId",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SettlerId",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Suggestions");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suggestions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
