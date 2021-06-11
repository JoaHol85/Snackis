using Microsoft.EntityFrameworkCore.Migrations;

namespace Snackis.Migrations
{
    public partial class v24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "UserImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_MessageId",
                table: "UserImages",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserImages_Messages_MessageId",
                table: "UserImages",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_Messages_MessageId",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_UserImages_MessageId",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "UserImages");
        }
    }
}
