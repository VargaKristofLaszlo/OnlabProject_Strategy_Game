using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class deleteSomeBuildings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_BarrackId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CityHallId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CityWallId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_FarmId",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_BarrackId",
                table: "Cities",
                column: "BarrackId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CityHallId",
                table: "Cities",
                column: "CityHallId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CityWallId",
                table: "Cities",
                column: "CityWallId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_FarmId",
                table: "Cities",
                column: "FarmId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_BarrackId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CityHallId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CityWallId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_FarmId",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_BarrackId",
                table: "Cities",
                column: "BarrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CityHallId",
                table: "Cities",
                column: "CityHallId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CityWallId",
                table: "Cities",
                column: "CityWallId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_FarmId",
                table: "Cities",
                column: "FarmId");
        }
    }
}
