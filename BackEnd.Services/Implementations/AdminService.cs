using BackEnd.Repositories;
using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Extensions;
using BackEnd.Services.Interfaces;
using Shared.Models.Request;
using Shared.Models.Response;
using System;
using System.Threading.Tasks;

namespace BackEnd.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly IMailService _mailService;

        public AdminService(IUnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;           
            _mailService = mailService;
        }

        public async Task<OperationResponse> BanUserAsync(UserBanRequest banRequest)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(banRequest.Username);

            if (user == null)
                return OperationResponse.Failed("User was not found");

            user.IsBanned = true;

            _mailService.SendEmail(user.Email, banRequest.CauseOfBan, banRequest.Message);

            await _unitOfWork.CommitChangesAsync();

            return OperationResponse.Succeeded($"{banRequest.Username}'s account has been banned");
        }        

        public async Task<OperationResponse> CreateBuildingUpgradeCostAsync(UpgradeCostCreationRequest request)
        {
            var previousMaxStage = await _unitOfWork.UpgradeCosts.FindMaxStage(request.BuildingName);

            if (previousMaxStage.HasValue)
                // Only be able to create an upgrade cost for the next stage
                if (previousMaxStage.Value + 1 != request.BuildingStage)
                    return OperationResponse.Failed("You can only creat an upgrade cost for the next stage");
                else
                if (request.BuildingStage != 1)
                    return OperationResponse.Failed("If you want to create an upgrade cost for a new building" +
                        " create a cost for stage 1 first");

            await _unitOfWork.UpgradeCosts.CreateAsync(request.ToBuildingUpgradeCostModel());

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return OperationResponse.Failed(e.Message);
            }

            return OperationResponse.Succeeded("Upgrade cost was created");
        }

        public async Task<OperationResponse> ModifyBuildingUpgradeCostAsync(UpgradeCostCreationRequest request)
        {
            var result = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.BuildingStage);

            result.ModifyValues(request);

            await _unitOfWork.CommitChangesAsync();

            return OperationResponse.Succeeded("Upgrade cost has been modified");
        }
    }    
}
