using Microsoft.EntityFrameworkCore.Migrations;

namespace Snackis.Migrations
{
    public partial class v16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_AspNetUsers_SnackisUserId1",
                table: "GroupMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessages_SnackisUserId1",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "SnackisUserId1",
                table: "GroupMessages");

            migrationBuilder.AlterColumn<string>(
                name: "SnackisUserId",
                table: "GroupMessages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_SnackisUserId",
                table: "GroupMessages",
                column: "SnackisUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_AspNetUsers_SnackisUserId",
                table: "GroupMessages",
                column: "SnackisUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_AspNetUsers_SnackisUserId",
                table: "GroupMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessages_SnackisUserId",
                table: "GroupMessages");

            migrationBuilder.AlterColumn<int>(
                name: "SnackisUserId",
                table: "GroupMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnackisUserId1",
                table: "GroupMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_SnackisUserId1",
                table: "GroupMessages",
                column: "SnackisUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_AspNetUsers_SnackisUserId1",
                table: "GroupMessages",
                column: "SnackisUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
