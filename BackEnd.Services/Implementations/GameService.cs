using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Interfaces;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly IdentityOptions _identityOptions;

        public GameService(IUnitOfWork unitOfWork, IdentityOptions identityOptions)
        {
            _unitOfWork = unitOfWork;
            _identityOptions = identityOptions;           
        }

        public async Task UpgradeBuilding(int cityIndex, string buildingName, int newStage)
        {
            var buildingBehaviour = CreateConcreteBuildingBehaviour(buildingName);

            //Check if the building is upgradeable
            if (await IsUpgradeable(buildingName, newStage) == false)
                return;

            //Get the city which contains the upgradeable building
            var user = await _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId);          
            var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(cityIndex).Id);

            //Get the upgrade cost for the building
            var upgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(buildingName, newStage);

            //Check if the city has enough resources to upgrade the building
            if (HasEnoughResources(city, upgradeCost) == false)
                return;

            //Upgrade the building
            buildingBehaviour.Upgrade(city, upgradeCost);
            PayUpgradeCost(city, upgradeCost);     
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

        private void PayUpgradeCost(City city, BuildingUpgradeCost upgradeCost) 
        {
            city.Resources.Population -= upgradeCost.UpgradeCost.Population;
            city.Resources.Wood -= upgradeCost.UpgradeCost.Wood;
            city.Resources.Silver -= upgradeCost.UpgradeCost.Silver;
            city.Resources.Stone -= upgradeCost.UpgradeCost.Stone;
        }

        public async Task DowngradeBuilding(int cityIndex, string buildingName, int newStage)
        {
            if (newStage < 2)
                return;

            var buildingBehaviour = CreateConcreteBuildingBehaviour(buildingName);

            //Get the city which contains the downgradeable building
            var user = await _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId);
            var city = await _unitOfWork.Cities.FindCityById(user.Cities.ElementAt(cityIndex).Id);

            //Get the upgrade cost for the building
            var upgradeCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost(buildingName, newStage);

            //Upgrade the building
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
    }
}
