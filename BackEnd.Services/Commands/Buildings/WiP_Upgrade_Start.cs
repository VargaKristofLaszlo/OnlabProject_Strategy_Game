using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using MediatR;
using Services.Exceptions;
using Services.Helpers;
using Services.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
namespace Services.Commands.Buildings
{
    public static class WiP_Upgrade_Start
    {
        public record Command(int CityIndex, string BuildingName, int NewStage, IIdentityContext IdentityContext) : IRequest<DateTime>;

        public class Handler : BuildingHandler, IRequestHandler<Command, DateTime>
        {
            private readonly IUnitOfWork _unitOfWork;           

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;         
            }

            public async Task<DateTime> Handle(Command request, CancellationToken cancellationToken)
            {
                ValidateBuildingName(request.BuildingName);

                if (await IsUpgradeable(request.BuildingName, request.NewStage) == false)
                    throw new BadRequestException("The building is not upgradeable");

                //Get the city which contains the upgradeable building
                var user = await _unitOfWork.Users.GetUserWithCities(request.IdentityContext.UserId);             
                var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(request.CityIndex).Id);
               
                var upgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.NewStage);

                ResourceCostRemoval.PayTheCost(city, upgradeCost.UpgradeCost);

                var finishTime = await _unitOfWork.HangFire.GetFinishTime(request.IdentityContext.UserId);
                var newFinishTime = finishTime.Add(upgradeCost.UpgradeTime);
                await _unitOfWork.HangFire.AddNewJob(request.IdentityContext.UserId, finishTime, newFinishTime
                    ,request.BuildingName, request.NewStage, request.CityIndex);

                await _unitOfWork.CommitChangesAsync();


                return finishTime.Add(upgradeCost.UpgradeTime);
            }

            private async Task<bool> IsUpgradeable(string buildingName, int newStage)
            {
                var maxStage = await _unitOfWork.UpgradeCosts.FindMaxStage(buildingName);
                return newStage <= maxStage;
            } 
        }
    }
}
