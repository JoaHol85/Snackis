using Microsoft.EntityFrameworkCore.Migrations;

namespace Snackis.Migrations
{
    public partial class v20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmileyMessageUsers_AspNetUsers_SnackisUserId1",
                table: "SmileyMessageUsers");

            migrationBuilder.DropIndex(
                name: "IX_SmileyMessageUsers_SnackisUserId1",
                table: "SmileyMessageUsers");

            migrationBuilder.DropColumn(
                name: "SnackisUserId1",
                table: "SmileyMessageUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SnackisUserId",
                table: "SmileyMessageUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SmileyMessageUsers_SnackisUserId",
                table: "SmileyMessageUsers",
                column: "SnackisUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmileyMessageUsers_AspNetUsers_SnackisUserId",
                table: "SmileyMessageUsers",
                column: "SnackisUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmileyMessageUsers_AspNetUsers_SnackisUserId",
                table: "SmileyMessageUsers");

            migrationBuilder.DropIndex(
                name: "IX_SmileyMessageUsers_SnackisUserId",
                table: "SmileyMessageUsers");

            migrationBuilder.AlterColumn<int>(
                name: "SnackisUserId",
                table: "SmileyMessageUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnackisUserId1",
                table: "SmileyMessageUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmileyMessageUsers_SnackisUserId1",
                table: "SmileyMessageUsers",
                column: "SnackisUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SmileyMessageUsers_AspNetUsers_SnackisUserId1",
                table: "SmileyMessageUsers",
                column: "SnackisUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
