using Game.Shared.Models.Request;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.IServices
{
    public interface IAdminService
    {
        [Post("/Ban")]
        Task BanUser([Body]UserBanRequest request);

        [Post("/Create/UpgradeCost")]
        Task CreateBuildingUpgradeCost([Body]UpgradeCostCreationRequest request);

        [Put("/Modify/UpgradeCost")]
        Task ModifyBuildingUpgradeCost(UpgradeCostCreationRequest request);

        [Put("/Moderate/Cityname")]
        Task ModerateCityName(CityNameModerationRequest request);

        [Put("/Modify/UnitCost")]
        Task ModifyUnitCost(UnitCostModificationRequest request);
    }
}
