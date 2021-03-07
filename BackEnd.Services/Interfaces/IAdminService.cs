using Game.Shared.Models.Request;
using System.Threading.Tasks;

namespace BackEnd.Services.Interfaces
{
    public interface IAdminService 
    {        
        Task CreateBuildingUpgradeCostAsync(UpgradeCostCreationRequest request);
        Task ModifyBuildingUpgradeCostAsync(UpgradeCostCreationRequest request);
        Task BanUserAsync(UserBanRequest banRequest);
        Task ModerateCityNameAsync(CityNameModerationRequest request);
        Task ModifyUnitCostAsync(UnitCostModificationRequest request);
    }
}
