using Shared.Models.Request;
using Shared.Models.Response;
using System.Threading.Tasks;

namespace BackEnd.Services.Interfaces
{
    public interface IAdminService 
    {        
        Task<OperationResponse> CreateBuildingUpgradeCostAsync(UpgradeCostCreationRequest request);
        Task<OperationResponse> ModifyBuildingUpgradeCostAsync(UpgradeCostCreationRequest request);
        Task<OperationResponse> BanUserAsync(UserBanRequest banRequest);
    }
}
