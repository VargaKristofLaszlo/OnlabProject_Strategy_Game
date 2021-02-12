using Game.Shared.Models;
using Shared.Models;
using Shared.Models.Response;
using System.Threading.Tasks;

namespace BackEnd.Services.Interfaces
{
    public interface IViewService
    {
        Task<CollectionResponse<Credentials>> GetUserCredentialsAsync(int pageNumber = 1, int pageSize = 10);
        Task<CollectionResponse<string>> GetCityNamesOfUser(string username, int pageNumber = 1, int pageSize = 10);
        Task<BuildingUpgradeCost> GetBuildingUpgradeCost(string buildingName, int buildingStage);
        Task<CollectionResponse<Unit>> GetUnitTypes(int pageNumber = 1, int pageSize = 10);
    }
}
