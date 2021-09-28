using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class AddedSpyReporttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpyReports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BarrackStage = table.Column<int>(type: "int", nullable: false),
                    CityHallStage = table.Column<int>(type: "int", nullable: false),
                    CityWallStage = table.Column<int>(type: "int", nullable: false),
                    FarmStage = table.Column<int>(type: "int", nullable: false),
                    SilverMineStage = table.Column<int>(type: "int", nullable: false),
                    StoneMineStage = table.Column<int>(type: "int", nullable: false),
                    LumberStage = table.Column<int>(type: "int", nullable: false),
                    WarehouseStage = table.Column<int>(type: "int", nullable: false),
                    CastleStage = table.Column<int>(type: "int", nullable: false),
                    TavernStage = table.Column<int>(type: "int", nullable: false),
                    Spearmans = table.Column<int>(type: "int", nullable: false),
                    Swordsmans = table.Column<int>(type: "int", nullable: false),
                    AxeFighers = table.Column<int>(type: "int", nullable: false),
                    Archers = table.Column<int>(type: "int", nullable: false),
                    LightCavalry = table.Column<int>(type: "int", nullable: false),
                    MountedArcher = table.Column<int>(type: "int", nullable: false),
                    HeavyCavalry = table.Column<int>(type: "int", nullable: false),
                    Noble = table.Column<int>(type: "int", nullable: false),
                    Successful = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpyReports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpyReports");
        }
    }
}
