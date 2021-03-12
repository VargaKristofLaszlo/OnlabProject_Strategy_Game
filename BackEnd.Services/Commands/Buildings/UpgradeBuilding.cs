using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using Services.Exceptions;
using Services.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Resources = Game.Shared.Models.Resources;

namespace Services.Commands.Buildings
{
    public static class UpgradeBuilding
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
                var buildingBehaviour = CreateConcreteBuildingBehaviour(request.BuildingName);

                //Check if the building is upgradeable
                if (await IsUpgradeable(request.BuildingName, request.NewStage) == false)
                    throw new BadRequestException("The building is not upgradeable");

                //Get the city which contains the upgradeable building
                var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);
                var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(request.CityIndex).Id);

                //Get the upgrade cost for the building
                var upgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.NewStage);
                var newUpgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(request.BuildingName, request.NewStage + 1);

                //Check if the city has enough resources to upgrade the building
                if (HasEnoughResources(city, upgradeCost) == false)
                    throw new BadRequestException("The city does not have enough resources");

                //Upgrade the building
                city = buildingBehaviour.Upgrade(city, newUpgradeCost);
                PayTheCost(city, upgradeCost.UpgradeCost);
                await _unitOfWork.CommitChangesAsync();

                if (newUpgradeCost == null)
                {
                    return new SuccessfulBuildingStageModification()
                    {
                        NewStage = request.NewStage,
                        NewUpgradeCost = null
                    };
                }

                return new SuccessfulBuildingStageModification()
                {
                    NewStage = request.NewStage,
                    NewUpgradeCost = new Resources
                    {
                        Wood = newUpgradeCost.UpgradeCost.Wood,
                        Silver = newUpgradeCost.UpgradeCost.Silver,
                        Stone = newUpgradeCost.UpgradeCost.Stone,
                        Population = newUpgradeCost.UpgradeCost.Population
                    }
                };
            }

            private async Task<bool> IsUpgradeable(string buildingName, int newStage)
            {
                var maxStage = await _unitOfWork.UpgradeCosts.FindMaxStage(buildingName);
                return newStage <= maxStage;
            }

            private bool HasEnoughResources(City city, BackEnd.Models.Models.BuildingUpgradeCost upgradeCost)
            {
                return
                    city.Resources.Population >= upgradeCost.UpgradeCost.Population &&
                    city.Resources.Silver >= upgradeCost.UpgradeCost.Silver &&
                    city.Resources.Stone >= upgradeCost.UpgradeCost.Stone &&
                    city.Resources.Wood >= upgradeCost.UpgradeCost.Wood;
            }

            private void PayTheCost(City city, BackEnd.Models.Models.Resources cost)
            {
                city.Resources.Population -= cost.Population;
                city.Resources.Wood -= cost.Wood;
                city.Resources.Silver -= cost.Silver;
                city.Resources.Stone -= cost.Stone;
            }
        }       
    }
}
