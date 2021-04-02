using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using MediatR;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Buildings
{
    public static class WiP_UpgradeProcess
    {
        public record Command(int CityIndex, string BuildingName, int NewStage, IIdentityContext IdentityContext,
            DateTime FinishTime) : IRequest<MediatR.Unit>;

        public class Handler : BuildingHandler, IRequestHandler<Command, MediatR.Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MediatR.Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var buildingBehaviour = CreateConcreteBuildingBehaviour(request.BuildingName);                

                //Get the city which contains the upgradeable building
                var user = await _unitOfWork.Users.GetUserWithCities(request.IdentityContext.UserId);
                var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(request.CityIndex).Id);

                //Get the upgrade cost for the building                
                var newUpgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.NewStage + 1);

                //Upgrade the building
                buildingBehaviour.Upgrade(city, newUpgradeCost);

                var job = await _unitOfWork.HangFire.GetJobByFinishTime(request.FinishTime);
                _unitOfWork.HangFire.RemoveJob(job);

                await _unitOfWork.CommitChangesAsync();

                return new MediatR.Unit();
            }
        }
    }
}
