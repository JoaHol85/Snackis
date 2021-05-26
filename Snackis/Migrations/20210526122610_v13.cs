using Microsoft.EntityFrameworkCore.Migrations;

namespace Snackis.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "Groups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
