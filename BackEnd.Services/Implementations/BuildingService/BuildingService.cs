using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Services.Exceptions;
using Services.Implementations.BuildingService.BuildingBehaviourImpl;
using Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementations.BuildingService
{
    public class BuildingService : IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityContext _identityContext;
       

        public BuildingService(IUnitOfWork unitOfWork, IIdentityContext identityOptions)
        {
            _unitOfWork = unitOfWork;
            _identityContext = identityOptions;
        }


        public async Task UpgradeBuilding(int cityIndex, string buildingName, int newStage)
        {
            var buildingBehaviour = CreateConcreteBuildingBehaviour(buildingName);

            //Check if the building is upgradeable
            if (await IsUpgradeable(buildingName, newStage) == false)
                throw new BadRequestException("The building is not upgradeable");

            //Get the city which contains the upgradeable building
            var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);
            var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(cityIndex).Id);

            //Get the upgrade cost for the building
            var upgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(buildingName, newStage);

            //Check if the city has enough resources to upgrade the building
            if (HasEnoughResources(city, upgradeCost) == false)
                throw new BadRequestException("The city does not have enough resources");

            //Upgrade the building
            buildingBehaviour.Upgrade(city, upgradeCost);
            PayTheCost(city, upgradeCost.UpgradeCost);
            await _unitOfWork.CommitChangesAsync();
        }

        private async Task<bool> IsUpgradeable(string buildingName, int newStage)
        {
            var maxStage = await _unitOfWork.UpgradeCosts.FindMaxStage(buildingName);
            return newStage <= maxStage;
        }

        private bool HasEnoughResources(City city, BuildingUpgradeCost upgradeCost)
        {
            return
                city.Resources.Population >= upgradeCost.UpgradeCost.Population &&
                city.Resources.Silver >= upgradeCost.UpgradeCost.Silver &&
                city.Resources.Stone >= upgradeCost.UpgradeCost.Stone &&
                city.Resources.Wood >= upgradeCost.UpgradeCost.Wood;
        }


        public async Task DowngradeBuilding(int cityIndex, string buildingName, int newStage)
        {
            if (newStage < 2)
                throw new BadRequestException("The building can not be downgraded any further");

            var buildingBehaviour = CreateConcreteBuildingBehaviour(buildingName);

            //Get the city which contains the downgradeable building
            var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);
            var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(cityIndex).Id);

            //Get the upgrade cost for the building
            var upgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(buildingName, newStage);

            //Downgrade the building
            buildingBehaviour.Downgrade(city, upgradeCost);
            await _unitOfWork.CommitChangesAsync();
        }

        private BuildingBehaviour CreateConcreteBuildingBehaviour(string buildingName)
        {
            switch (buildingName)
            {
                case "Barrack":
                    return new BarrackBehaviour();
                case "CityHall":
                    return new CityHallBehaviour();
                case "CityWall":
                    return new CityWallBehaviour();
                case "Farm":
                    return new FarmBehaviour();
                case "SilverMine":
                    return new SilverMineBehaviour();
                case "StoneMine":
                    return new StoneMineBehaviour();
                case "Lumber":
                    return new LumberBehaviour();
                case "Warehouse":
                    return new WarehouseBehaviour();
                default:
                    throw new NotFoundException();
            }
        }

        private void PayTheCost(City city, Resources cost)
        {
            city.Resources.Population -= cost.Population;
            city.Resources.Wood -= cost.Wood;
            city.Resources.Silver -= cost.Silver;
            city.Resources.Stone -= cost.Stone;
        }
    }
}
