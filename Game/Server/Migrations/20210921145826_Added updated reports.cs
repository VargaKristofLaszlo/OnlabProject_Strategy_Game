using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class Addedupdatedreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NobleAttackerCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NobleAttackerCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NobleDefenderCountAfter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NobleDefenderCountBefore",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NobleAttackerCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NobleAttackerCountBefore",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NobleDefenderCountAfter",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NobleDefenderCountBefore",
                table: "Reports");
        }
    }
}
