using BackEnd.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DataSeeding
{
    public class BuildingUpgradeCostSeeding
    {
        private readonly ApplicationDbContext _db;
        public BuildingUpgradeCostSeeding(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SeedBuildingUpgradeCost()
        {
            await CreateBuildingUpgradeCost("Barrack");
            await CreateBuildingUpgradeCost("CityHall");
            await CreateBuildingUpgradeCost("CityWall");
            await CreateBuildingUpgradeCost("Farm");
            await CreateBuildingUpgradeCost("Warehouse");
            await CreateBuildingUpgradeCost("SilverMine");
            await CreateBuildingUpgradeCost("StoneMine");
            await CreateBuildingUpgradeCost("Lumber");
            await CreateBuildingUpgradeCost("Castle");
            await CreateExtraStage("Barrack", 2);
        }

        private async Task CreateBuildingUpgradeCost(string buildingName)
        {
            //Avoid adding duplicates
            var cost = await _db.MaxBuildingStages
              .Where(max => max.BuildingName.Equals(buildingName))
              .FirstOrDefaultAsync();

            if (cost != null)
                return;

            await _db.BuildingUpgradeCosts.AddAsync(new BuildingUpgradeCost
            {
                UpgradeCost = new Resources
                {
                    Wood = 10,
                    Silver = 10,
                    Stone = 10,
                    Population = 5
                },
                UpgradeTime = new TimeSpan(0, 0, 10),
                BuildingName = buildingName,
                BuildingStage = 1
            });

            await _db.MaxBuildingStages.AddAsync(new MaxBuildingStage
            {
                BuildingName = buildingName,
                MaxStage = 1
            });

            await _db.SaveChangesAsync();
        }

        private async Task CreateExtraStage(string buildingName, int stage)
        {
            //Avoid adding duplicates
            var maxBuildingStage = await _db.MaxBuildingStages
              .Where(max => max.BuildingName.Equals(buildingName))
              .FirstOrDefaultAsync();

            if (maxBuildingStage == null)
                return;

            if (maxBuildingStage.MaxStage + 1 != stage)
                return;

            await _db.BuildingUpgradeCosts.AddAsync(new BuildingUpgradeCost
            {
                UpgradeCost = new Resources
                {
                    Wood = 20,
                    Silver = 20,
                    Stone = 20,
                    Population = 5
                },
                UpgradeTime = new TimeSpan(0, 0, 15),
                BuildingName = buildingName,
                BuildingStage = stage
            });

            maxBuildingStage.MaxStage = stage;

            await _db.SaveChangesAsync();
        }


    }
}
