using AutoMapper;
using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Extensions;
using BackEnd.Services.Interfaces;
using Game.Shared.Models.Request;
using Services.Exceptions;
using Shared.Models.Request;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork unitOfWork, IMailService mailService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
        }

        public async Task BanUserAsync(UserBanRequest banRequest)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(banRequest.Username);

            if (user == null)
                throw new NotFoundException();

            user.IsBanned = true;

            _mailService.SendEmail(user.Email, banRequest.CauseOfBan, banRequest.Message);

            await _unitOfWork.CommitChangesAsync();
        }

        public async Task CreateBuildingUpgradeCostAsync(UpgradeCostCreationRequest request)
        {
            var previousMaxStage = await _unitOfWork.UpgradeCosts.FindMaxStage(request.BuildingName);

            if (previousMaxStage.HasValue)
            {
                // Only be able to create an upgrade cost for the next stage
                if (previousMaxStage.Value + 1 != request.BuildingStage)
                    throw new BadRequestException("You can only creat an upgrade cost for the next stage");
            }
            else
            {
                if (request.BuildingStage != 1)
                    throw new BadRequestException("If you want to create an upgrade cost for a new building" +
                        " create a cost for stage 1 first");
            }

            await _unitOfWork.UpgradeCosts.CreateAsync(_mapper.Map<Models.Models.BuildingUpgradeCost>(request));
            await _unitOfWork.CommitChangesAsync();
        }

        public async Task ModerateCityNameAsync(CityNameModerationRequest request)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(request.Username);

            if (user == null)
                throw new NotFoundException();

            var city = user.Cities
                .Where(city => city.CityName.Equals(request.OldCityName))
                .FirstOrDefault();

            if (city == null)
                throw new NotFoundException();

            city.CityName = request.NewCityName;

            await _unitOfWork.CommitChangesAsync();
        }

        public async Task ModifyBuildingUpgradeCostAsync(UpgradeCostCreationRequest request)
        {
            var result = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.BuildingStage);

            if (result == null)
                throw new NotFoundException();

            result.ModifyValues(request);

            await _unitOfWork.CommitChangesAsync();
        }

        public async Task ModifyUnitCostAsync(UnitCostModificationRequest request)
        {
            var result = await _unitOfWork.Units.FindUnitByName(request.Name);

            if (result == null)
                throw new NotFoundException();

            result.UnitCost.Wood = request.Wood;
            result.UnitCost.Stone = request.Stone;
            result.UnitCost.Silver = request.Silver;
            result.UnitCost.Population = request.Population;

            await _unitOfWork.CommitChangesAsync();
        }
    }
}
