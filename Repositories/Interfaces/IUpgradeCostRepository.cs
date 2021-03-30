using BackEnd.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Interfaces
{
    public interface IUpgradeCostRepository
    {
        Task CreateAsync(BuildingUpgradeCost upgradeCost);
        Task<BuildingUpgradeCost> FindUpgradeCost(string buildingName, int buildingStage);
        Task<List<BuildingUpgradeCost>> FindBuildingUpgradeCostsByName(string buildingName);
        Task<int?> FindMaxStage(string buildingName);
    }
}