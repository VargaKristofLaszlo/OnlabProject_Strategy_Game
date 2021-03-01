using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class addedClientCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Barracks_BarrackId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_CityHalls_CityHallId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_CityWalls_CityWallId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Farms_FarmId",
                table: "Cities");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Barracks_BarrackId",
                table: "Cities",
                column: "BarrackId",
                principalTable: "Barracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_CityHalls_CityHallId",
                table: "Cities",
                column: "CityHallId",
                principalTable: "CityHalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_CityWalls_CityWallId",
                table: "Cities",
                column: "CityWallId",
                principalTable: "CityWalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Farms_FarmId",
                table: "Cities",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Barracks_BarrackId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_CityHalls_CityHallId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_CityWalls_CityWallId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Farms_FarmId",
                table: "Cities");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Barracks_BarrackId",
                table: "Cities",
                column: "BarrackId",
                principalTable: "Barracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_CityHalls_CityHallId",
                table: "Cities",
                column: "CityHallId",
                principalTable: "CityHalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_CityWalls_CityWallId",
                table: "Cities",
                column: "CityWallId",
                principalTable: "CityWalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Farms_FarmId",
                table: "Cities",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
