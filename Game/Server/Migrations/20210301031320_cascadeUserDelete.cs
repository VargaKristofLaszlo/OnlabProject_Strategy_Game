using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class cascadeUserDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_UserId",
                table: "Cities");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_UserId",
                table: "Cities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_UserId",
                table: "Cities");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_UserId",
                table: "Cities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
