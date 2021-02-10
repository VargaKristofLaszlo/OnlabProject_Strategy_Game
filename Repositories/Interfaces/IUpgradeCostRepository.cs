using BackEnd.Models.Models;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Interfaces
{
    public interface IUpgradeCostRepository
    {
        Task CreateAsync(BuildingUpgradeCost upgradeCost);
        Task<BuildingUpgradeCost> FindUpgradeCost(string buildingName, int buildingStage);
        Task<int?> FindMaxStage(string buildingName);
    }
}