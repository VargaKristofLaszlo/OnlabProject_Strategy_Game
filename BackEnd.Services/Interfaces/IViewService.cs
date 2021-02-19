using Game.Shared.Models;
using Shared.Models;
using Shared.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Services.Interfaces
{
    public interface IViewService
    {
        Task<CollectionResponse<Credentials>> GetUserCredentialsAsync(int pageNumber = 1, int pageSize = 10);
        Task<CollectionResponse<string>> GetCityNamesOfUser(int pageNumber = 1, int pageSize = 10);
        Task<BuildingUpgradeCost> GetBuildingUpgradeCost(string buildingName, int buildingStage);
        Task<CollectionResponse<Unit>> GetUnitTypes(int pageNumber = 1, int pageSize = 10);
        Task<CityDetails> GetCityDetails(int cityIndex);
        Task<IEnumerable<Unit>> GetProducibleUnitTypes(int cityIndex);
        Task<CityResources> GetResourcesOfCity(int cityIndex);        
    }
}
