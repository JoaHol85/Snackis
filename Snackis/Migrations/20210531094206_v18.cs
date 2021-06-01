using Microsoft.EntityFrameworkCore.Migrations;

namespace Snackis.Migrations
{
    public partial class v18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmileyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HappySmiley = table.Column<int>(type: "int", nullable: false),
                    LaughingSmiley = table.Column<int>(type: "int", nullable: false),
                    SadSmiley = table.Column<int>(type: "int", nullable: false),
                    AngrySmiley = table.Column<int>(type: "int", nullable: false),
                    ThumbUp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmileyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmileyInfos_Messages_Id",
                        column: x => x.Id,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmileyMessageUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmileyNumber = table.Column<int>(type: "int", nullable: false),
                    SmileyInfoId = table.Column<int>(type: "int", nullable: false),
                    SnackisUserId = table.Column<int>(type: "int", nullable: false),
                    SnackisUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmileyMessageUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmileyMessageUsers_AspNetUsers_SnackisUserId1",
                        column: x => x.SnackisUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmileyMessageUsers_SmileyInfos_SmileyInfoId",
                        column: x => x.SmileyInfoId,
                        principalTable: "SmileyInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmileyMessageUsers_SmileyInfoId",
                table: "SmileyMessageUsers",
                column: "SmileyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SmileyMessageUsers_SnackisUserId1",
                table: "SmileyMessageUsers",
                column: "SnackisUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmileyMessageUsers");

            migrationBuilder.DropTable(
                name: "SmileyInfos");
        }
    }
}
