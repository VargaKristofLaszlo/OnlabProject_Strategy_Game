using Game.Shared.Models;
using Game.Shared.Models.Response;
using Refit;
using Shared.Models;
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
        Task<List<string>> GetCityNamesOfUser();

        [Get("/Units")]
        Task<CollectionResponse<Unit>> GetUnitTypes(int pageNumber, int pageSize);

        [Get("/City")]
        Task<CityDetails> GetCityDetails(int cityIndex);

        [Get("/Units/Producible")]
        Task<IEnumerable<Unit>> GetProducibleUnits(int cityIndex);

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
    }
}
