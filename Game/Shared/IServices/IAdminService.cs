using Game.Shared.Models.Request;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.IServices
{
    public interface IAdminService
    {
        [Post("/Ban")]
        Task BanUser([Body]UserBanRequest request);

        [Post("/Create/UpgradeCost")]
        Task<HttpResponseMessage> CreateBuildingUpgradeCost(UpgradeCostCreationRequest request);

        [Put("/Modify/UpgradeCost")]
        Task<HttpResponseMessage> ModifyBuildingUpgradeCost(UpgradeCostCreationRequest request);

        [Put("/Moderate/Cityname")]
        Task<HttpResponseMessage> ModerateCityName(CityNameModerationRequest request);

        [Put("/Modify/UnitCost")]
        Task<HttpResponseMessage> ModifyUnitCost(UnitCostModificationRequest request);
    }
}
