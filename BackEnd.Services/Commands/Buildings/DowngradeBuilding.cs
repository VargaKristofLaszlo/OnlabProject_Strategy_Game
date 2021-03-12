using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using Services.Exceptions;
using Services.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Buildings
{
    public static class DowngradeBuilding
    {
        public record Command(int CityIndex, string BuildingName, int NewStage) : IRequest<SuccessfulBuildingStageModification>;

        public class Handler : BuildingHandler, IRequestHandler<Command, SuccessfulBuildingStageModification>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityOptions)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityOptions;
            }


            public async Task<SuccessfulBuildingStageModification> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.NewStage < 0)
                    throw new BadRequestException("The building can not be downgraded any further");

                var buildingBehaviour = CreateConcreteBuildingBehaviour(request.BuildingName);

                //Get the city which contains the downgradeable building
                var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);
                var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(request.CityIndex).Id);

                //Get the upgrade cost for the building           
                var upgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.NewStage + 1);

                city.Resources.Population += upgradeCost.UpgradeCost.Population;


                //Downgrade the building
                city = buildingBehaviour.Downgrade(city, upgradeCost);
                await _unitOfWork.CommitChangesAsync();
                return new SuccessfulBuildingStageModification()
                {
                    NewStage = request.NewStage,
                    NewUpgradeCost = new Resources
                    {
                        Wood = upgradeCost.UpgradeCost.Wood,
                        Silver = upgradeCost.UpgradeCost.Silver,
                        Stone = upgradeCost.UpgradeCost.Stone,
                        Population = upgradeCost.UpgradeCost.Population
                    }
                };
            }
        }
    }
}
