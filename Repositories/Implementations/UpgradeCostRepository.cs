using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                previousMaxStage.MaxStage += 1;

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
    }
}