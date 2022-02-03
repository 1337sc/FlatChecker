using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatChecker.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestion_User_OwnerId",
                table: "Suggestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suggestion",
                table: "Suggestion");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Suggestion",
                newName: "Suggestions");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestion_OwnerId",
                table: "Suggestions",
                newName: "IX_Suggestions_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suggestions",
                table: "Suggestions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MinsFromPublicTransportMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicTransportType = table.Column<int>(type: "int", nullable: false),
                    Mins = table.Column<int>(type: "int", nullable: false),
                    SuggestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinsFromPublicTransportMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinsFromPublicTransportMaps_Suggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "Suggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MinsFromPublicTransportMaps_SuggestionId",
                table: "MinsFromPublicTransportMaps",
                column: "SuggestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Users_OwnerId",
                table: "Suggestions",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Users_OwnerId",
                table: "Suggestions");

            migrationBuilder.DropTable(
                name: "MinsFromPublicTransportMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suggestions",
                table: "Suggestions");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Suggestions",
                newName: "Suggestion");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestions_OwnerId",
                table: "Suggestion",
                newName: "IX_Suggestion_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suggestion",
                table: "Suggestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestion_User_OwnerId",
                table: "Suggestion",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
