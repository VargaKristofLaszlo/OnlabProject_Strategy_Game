using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class newcolumntoreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StolenSilverAmount",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StolenStoneAmount",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StolenWoodAmount",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StolenSilverAmount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "StolenStoneAmount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "StolenWoodAmount",
                table: "Reports");
        }
    }
}
