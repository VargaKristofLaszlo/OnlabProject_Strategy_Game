using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class asdad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cities_WarehouseId",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "ResourceProductions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "Farms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "CityWalls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "CityHalls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityId",
                table: "Barracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceProductions_CityId",
                table: "ResourceProductions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_WarehouseId",
                table: "Cities",
                column: "WarehouseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceProductions_Cities_CityId",
                table: "ResourceProductions",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceProductions_Cities_CityId",
                table: "ResourceProductions");

            migrationBuilder.DropIndex(
                name: "IX_ResourceProductions_CityId",
                table: "ResourceProductions");

            migrationBuilder.DropIndex(
                name: "IX_Cities_WarehouseId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ResourceProductions");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "CityWalls");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "CityHalls");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Barracks");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_WarehouseId",
                table: "Cities",
                column: "WarehouseId");
        }
    }
}
