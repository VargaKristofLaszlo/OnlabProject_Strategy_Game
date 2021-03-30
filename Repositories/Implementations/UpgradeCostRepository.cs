using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Implementations
{
    public class UpgradeCostRepository : IUpgradeCostRepository
    {
        private readonly ApplicationDbContext _db;

        public UpgradeCostRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(BuildingUpgradeCost upgradeCost)
        {
            await _db.BuildingUpgradeCosts.AddAsync(upgradeCost);
            var previousMaxStage = await _db.MaxBuildingStages
                .FirstOrDefaultAsync(cost => cost.BuildingName.Equals(upgradeCost.BuildingName));

            if (previousMaxStage != null) 
            {
                previousMaxStage.MaxStage += 1;
                switch (upgradeCost.BuildingName)
                {
                    case "CityWall":
                        var cityhalls = await _db.CityHalls
                            .Where(x => x.BuildingCostId == null)
                            .ToListAsync();
                        foreach (var cityHall in cityhalls)
                        {
                            cityHall.BuildingCostId = upgradeCost.Id;
                            cityHall.UpgradeCost = upgradeCost;
                        }
                        break;
                    case "Lumber":
                        var lumbers = await _db.ResourceProductions
                           .Where(x => x.BuildingCostId == null && x.BuildingName.Equals("Lumber"))
                           .ToListAsync();
                        foreach (var lumber in lumbers)
                        {
                            lumber.BuildingCostId = upgradeCost.Id;
                            lumber.UpgradeCost = upgradeCost;
                        }
                        break;
                    case "Farm":
                        var farms = await _db.Farms
                          .Where(x => x.BuildingCostId == null)
                          .ToListAsync();
                        foreach (var farm in farms)
                        {
                            farm.BuildingCostId = upgradeCost.Id;
                            farm.UpgradeCost = upgradeCost;
                        }
                        break;
                    case "Barrack":
                        var barracks = await _db.Barracks
                            .Where(x => x.BuildingCostId == null)
                            .ToListAsync();
                        foreach (var barrack in barracks)
                        {
                            barrack.BuildingCostId = upgradeCost.Id;
                            barrack.UpgradeCost = upgradeCost;
                        }
                        break;
                    case "Warehouse":
                        var warehouses = await _db.Warehouses
                           .Where(x => x.BuildingCostId == null)
                           .ToListAsync();
                        foreach (var warehouse in warehouses)
                        {
                            warehouse.BuildingCostId = upgradeCost.Id;
                            warehouse.UpgradeCost = upgradeCost;
                        }
                        break;
                    case "CityHall":
                        var cityHalls = await _db.CityHalls
                             .Where(x => x.BuildingCostId == null)
                             .ToListAsync();
                        foreach (var cityHall in cityHalls)
                        {
                            cityHall.BuildingCostId = upgradeCost.Id;
                            cityHall.UpgradeCost = upgradeCost;
                        }
                        break;
                    case "SilverMine":
                        var silverMines = await _db.ResourceProductions
                         .Where(x => x.BuildingCostId == null && x.BuildingName.Equals("SilverMine"))
                         .ToListAsync();
                        foreach (var silverMine in silverMines)
                        {
                            silverMine.BuildingCostId = upgradeCost.Id;
                            silverMine.UpgradeCost = upgradeCost;
                        }
                        break;
                    case "StoneMine":
                        var stoneMines = await _db.ResourceProductions
                         .Where(x => x.BuildingCostId == null && x.BuildingName.Equals("StoneMine"))
                         .ToListAsync();
                        foreach (var stoneMine in stoneMines)
                        {
                            stoneMine.BuildingCostId = upgradeCost.Id;
                            stoneMine.UpgradeCost = upgradeCost;
                        }
                        break;
                    default:
                        break;
                }
            }
               

            else
            {
                previousMaxStage = new MaxBuildingStage
                {
                    BuildingName = upgradeCost.BuildingName,
                    MaxStage = 1
                };

                await _db.MaxBuildingStages.AddAsync(previousMaxStage);
            }
        }

        public async Task<int?> FindMaxStage(string buildingName)
        {
            var res = await _db.MaxBuildingStages.FirstOrDefaultAsync(building => building.BuildingName.Equals(buildingName));

            if (res == null)
                return null;

            return res.MaxStage;
        }

        public async Task<BuildingUpgradeCost> FindUpgradeCost(string buildingName, int buildingStage)
        {
            return await _db.BuildingUpgradeCosts 
                .Include(cost => cost.UpgradeCost)
                .FirstOrDefaultAsync(cost => cost.BuildingName.Equals(buildingName) && cost.BuildingStage == buildingStage);
        }

        public async Task<List<BuildingUpgradeCost>> FindBuildingUpgradeCostsByName(string buildingName) 
        {
            return  await _db.BuildingUpgradeCosts
                .Where(x => x.BuildingName.Equals(buildingName))
                .ToListAsync();
        }
    }
}