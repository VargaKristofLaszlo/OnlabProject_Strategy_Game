using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using Services.Exceptions;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityOptions _identityOptions;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IdentityOptions identityOptions, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identityOptions = identityOptions;
            _mapper = mapper;
        }

        public async Task UpgradeBuilding(int cityIndex, string buildingName, int newStage)
        {
            var buildingBehaviour = CreateConcreteBuildingBehaviour(buildingName);

            //Check if the building is upgradeable
            if (await IsUpgradeable(buildingName, newStage) == false)
                throw new BadRequestException("The building is not upgradeable");

            //Get the city which contains the upgradeable building
            var user = await _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId);
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
            var user = await _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId);
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

        public async Task ProduceUnits(UnitProductionRequest request)
        {
            var city = await GetCityByCityIndex(request.CityIndex, _identityOptions.UserId);

            var producibleUnits = await _unitOfWork.Units.GetProducibleUnitTypes(city.Barrack.Stage);

            if (CheckIfUnitIsProducible(producibleUnits, request.NameOfUnitType) == false)
                throw new NotFoundException();           

            var unit = producibleUnits.Where(unit => unit.Name.Equals(request.NameOfUnitType)).First();

            var productionCost = new Resources
            {
                Population = request.Amount * unit.UnitCost.Population,
                Silver = request.Amount * unit.UnitCost.Silver,
                Stone = request.Amount * unit.UnitCost.Stone,
                Wood = request.Amount * unit.UnitCost.Wood
            };

            if (CheckIfCityHasEnoughResources(city, productionCost) == false)
                throw new BadRequestException("The city does not have enough resources");

            PayTheCost(city, productionCost);

            var unitsOfThisTypeInCity = await _unitOfWork.Units.GetUnitsInCityByUnitId(unit.Id);

            //Create a new entry in the db
            if (unitsOfThisTypeInCity == null)
            {
                await _unitOfWork.Units.InsertNewEntryToUnitsInCity(new UnitsInCity
                {
                    Barrack = city.Barrack,
                    BarrackId = city.Barrack.Id,
                    Unit = unit,
                    UnitId = unit.Id,
                    Amount = request.Amount
                });
            }
            //Increase the amount in the city
            else
                unitsOfThisTypeInCity.Amount += request.Amount;

            await _unitOfWork.CommitChangesAsync();
        }

        private async Task<City> GetCityByCityIndex(int cityIndex, string userId)
        {
            var user = await _unitOfWork.Users.GetUserWithCities(userId);

            if (user == null)
                throw new NotFoundException();

            var city = await _unitOfWork.Cities.FindCityById(user.Cities[cityIndex].Id);

            if (city == null)
                throw new NotFoundException();

            return city;
        }


        private bool CheckIfUnitIsProducible(IEnumerable<Unit> units, string nameOfUnitType)
        {
            foreach (var item in units)
            {
                if (item.Name.Equals(nameOfUnitType))
                    return true;
            }
            return false;
        }

        private void PayTheCost(City city, Resources cost) 
        {
            city.Resources.Population -= cost.Population;
            city.Resources.Wood -= cost.Wood;
            city.Resources.Silver -= cost.Silver;
            city.Resources.Stone -= cost.Stone;
        }

        private bool CheckIfCityHasEnoughResources(City city, Resources cost) 
        {
            return
                city.Resources.Population >= cost.Population &&
                city.Resources.Silver >= cost.Silver &&
                city.Resources.Stone >= cost.Stone &&
                city.Resources.Wood >= cost.Wood;
        }

        public async Task SendResourcesToOtherPlayer(SendResourceToOtherPlayerRequest request)
        {
            var senderCity = await GetCityByCityIndex(request.fromCityIndex, _identityOptions.UserId);

            var receivingUser = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(request.toUserName);
            var receivingCity = await GetCityByCityIndex(request.toCityIndex, receivingUser.Id);

            var sentResources = _mapper.Map<Resources>(request);

            if (CheckIfCityHasEnoughResources(senderCity, sentResources) == false)
                throw new BadRequestException("The city does not have enough resources");

            var silverAmount = receivingCity.Resources.Silver + sentResources.Silver;
            var stoneAmount = receivingCity.Resources.Stone + sentResources.Stone;
            var woodAmount = receivingCity.Resources.Wood + sentResources.Wood;

            CheckWarehouseCapacity(ref stoneAmount, ref silverAmount, ref woodAmount, receivingCity.Warehouse);

            receivingCity.Resources.Wood = woodAmount;
            receivingCity.Resources.Silver = silverAmount;
            receivingCity.Resources.Stone = stoneAmount;

            PayTheCost(senderCity, sentResources);
            await _unitOfWork.CommitChangesAsync();
        }

        private void CheckWarehouseCapacity(ref int stoneAmount, ref int silverAmount, ref int woodAmount, Warehouse warehouse)
        {
            if (stoneAmount > warehouse.MaxStoneStorageCapacity)
                stoneAmount = warehouse.MaxStoneStorageCapacity;

            if (silverAmount > warehouse.MaxSilverStorageCapacity)
                silverAmount = warehouse.MaxSilverStorageCapacity;

            if (woodAmount > warehouse.MaxWoodStorageCapacity)
                woodAmount = warehouse.MaxWoodStorageCapacity;
        }
    }
}
