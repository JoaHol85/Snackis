using Microsoft.EntityFrameworkCore.Migrations;

namespace Snackis.Migrations
{
    public partial class v17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SnackisUserId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SnackisUserId",
                table: "Messages",
                column: "SnackisUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SnackisUserId",
                table: "Messages",
                column: "SnackisUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SnackisUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SnackisUserId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "SnackisUserId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
