using Microsoft.EntityFrameworkCore.Migrations;

namespace Snackis.Migrations
{
    public partial class _26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupMessageId",
                table: "MessageImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageImages_GroupMessageId",
                table: "MessageImages",
                column: "GroupMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageImages_GroupMessages_GroupMessageId",
                table: "MessageImages",
                column: "GroupMessageId",
                principalTable: "GroupMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageImages_GroupMessages_GroupMessageId",
                table: "MessageImages");

            migrationBuilder.DropIndex(
                name: "IX_MessageImages_GroupMessageId",
                table: "MessageImages");

            migrationBuilder.DropColumn(
                name: "GroupMessageId",
                table: "MessageImages");
        }
    }
}
