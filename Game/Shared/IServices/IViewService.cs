using Game.Shared.Models;
using Game.Shared.Models.Response;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Shared.IServices
{
    public interface IViewService
    {
        [Get("/Credentials")]
        Task<CollectionResponse<Credentials>> GetUserCredentials(int pageNumber, int pageSize);

        [Get("/Building/UpgradeCost")]
        Task<CollectionResponse<Credentials>> GetBuildingUpgradeCost(string buildingName, int buildingStage);

        [Get("/CityNames")]
        Task<IEnumerable<string>> GetCityNamesOfLoggedInUser();

        [Get("/CityNames/{id}")]
        Task<IEnumerable<string>> GetCityNamesByUserId(string id);

        [Get("/Units")]
        Task<CollectionResponse<Unit>> GetUnitTypes(int pageNumber, int pageSize);

        [Get("/City")]
        Task<CityDetails> GetCityDetails(int cityIndex);

        [Get("/Units/{unitName}")]
        Task<Resources> GetUnitCostByName(string unitName);

        [Get("/Building/{buildingName}/Upgradecost")]
        Task<List<Models.Request.UpgradeCostCreationRequest>> GetBuildingUpgradeCostsByName(string buildingName);

        [Get("/City/Resources")]
        Task<CityResources> GetResourcesOfTheCity(int cityIndex);

        [Get("/Warehouse/Capacity")]
        Task <WarehouseCapacity> GetWarehouseCapacity(int cityIndex);

        [Get("/UnitsOfCity")]
        Task<UnitsOfTheCity> GetUnitsOfCity(int cityIndex);

        [Get("/Users/Others/Cities")]
        Task<CollectionResponse<CityPagingData>> GetOtherUsersCities(int pageNumber, int pageSize);

        [Get("/Reports")]
        Task<CollectionResponse<Report>> GetReports(int pageNumber, int pageSize);

        [Get("/BuildingQueue/{userId}")]
        Task<BuildingQueue> GetBuildingQueueById(string userId);
        [Get("/UnitRecruitQueue/{userId}")]
        Task<UnitQueue> GetUnitQueueById(string userId);
    }
}
