using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Server.Migrations
{
    public partial class newRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttackerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DefenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AttackingForcesId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SurvivedAttackingForcesId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DefendingForcesId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SurvivedDefendingForcesId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_AttackerId",
                        column: x => x.AttackerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_DefenderId",
                        column: x => x.DefenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_UnitsInCities_AttackingForcesId",
                        column: x => x.AttackingForcesId,
                        principalTable: "UnitsInCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_UnitsInCities_DefendingForcesId",
                        column: x => x.DefendingForcesId,
                        principalTable: "UnitsInCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_UnitsInCities_SurvivedAttackingForcesId",
                        column: x => x.SurvivedAttackingForcesId,
                        principalTable: "UnitsInCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_UnitsInCities_SurvivedDefendingForcesId",
                        column: x => x.SurvivedDefendingForcesId,
                        principalTable: "UnitsInCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AttackerId",
                table: "Reports",
                column: "AttackerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AttackingForcesId",
                table: "Reports",
                column: "AttackingForcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DefenderId",
                table: "Reports",
                column: "DefenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DefendingForcesId",
                table: "Reports",
                column: "DefendingForcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SurvivedAttackingForcesId",
                table: "Reports",
                column: "SurvivedAttackingForcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SurvivedDefendingForcesId",
                table: "Reports",
                column: "SurvivedDefendingForcesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
