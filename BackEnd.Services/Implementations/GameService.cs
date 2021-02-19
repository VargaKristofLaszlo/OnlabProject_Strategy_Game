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

            (bool Producible, Unit Type) unitCheck = CheckIfUnitIsProducible(producibleUnits, request.NameOfUnitType);

            if (unitCheck.Producible == false)
                throw new NotFoundException();

            var productionCost = new Resources
            {
                Population = request.Amount * unitCheck.Type.UnitCost.Population,
                Silver = request.Amount * unitCheck.Type.UnitCost.Silver,
                Stone = request.Amount * unitCheck.Type.UnitCost.Stone,
                Wood = request.Amount * unitCheck.Type.UnitCost.Wood
            };

            if (CheckIfCityHasEnoughResources(city, productionCost) == false)
                throw new BadRequestException("The city does not have enough resources");

            PayTheCost(city, productionCost);

            var unitsOfThisTypeInCity = await _unitOfWork.Units.GetUnitsInCityByUnitId(unitCheck.Type.Id);

            //Create a new entry in the db
            if (unitsOfThisTypeInCity == null)
            {
                await _unitOfWork.Units.InsertNewEntryToUnitsInCity(new UnitsInCity
                {
                    Barrack = city.Barrack,
                    BarrackId = city.Barrack.Id,
                    Unit = unitCheck.Type,
                    UnitId = unitCheck.Type.Id,
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


        private (bool Producible, Unit Type) CheckIfUnitIsProducible(IEnumerable<Unit> units, string nameOfUnitType)
        {
            foreach (var item in units)
            {
                if (item.Name.Equals(nameOfUnitType))
                    return (true, item);
            }
            return (false, null);
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
            var senderCity = await GetCityByCityIndex(request.FromCityIndex, _identityOptions.UserId);

            var receivingUser = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(request.ToUserName);
            var receivingCity = await GetCityByCityIndex(request.ToCityIndex, receivingUser.Id);

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

        public async Task AttackOtherCity(AttackRequest request)
        {
            var attackingCity =  _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId).Result.Cities.ElementAt(request.AttackerCityIndex);

            var totalStrengths = CalculateAttackerUnitTypeStrength(request);

            int strengthSum = totalStrengths.infantryTotalStrength + totalStrengths.cavalryTotalStrength + totalStrengths.archerTotalStrength;

            double infantryStrengthPercent = totalStrengths.infantryTotalStrength / strengthSum;
            double cavalryStrengthPercent = totalStrengths.cavalryTotalStrength / strengthSum;
            double archerStrengthPercent = totalStrengths.archerTotalStrength / strengthSum;

            //Get the defending city's data
            var defendingUser = await _unitOfWork.Users.GetUserWithCities(request.AttackedUsername);
            if (defendingUser == null)
                throw new NotFoundException();

            var defendingUnits = 
                await _unitOfWork.Units.GetUnitsInCityByBarrackId(defendingUser.Cities.ElementAt(request.AttackedCityIndex).BarrackId);


            //InfantryDefensePhase
            double infantryDefenseValue = GetInfantryDefenseTotal(defendingUnits, infantryStrengthPercent);
            double infantryAttackValue = strengthSum * infantryStrengthPercent;

            //The defenders won
            if (infantryDefenseValue > infantryAttackValue)
            {
                //The attacker lost all attacking units
                FallenUnits attackersFallenUnits = new FallenUnits
                {
                    Spearman = request.SpearmanAmount,
                    Swordsman = request.SwordsmanAmount,
                    Archer = request.ArcherAmount,
                    AxeFighter = request.AxeFighterAmount,
                    MountedArcher = request.MountedArcherAmount,
                    LightCavalry = request.LightCavalryAmount,
                    HeavyCavalry = request.HeavyCavalryAmount
                };
                await RemoveTheFallenUnits(attackingCity.Barrack.Id, attackersFallenUnits);

                //TODO the defender lost a portion of it's units
                

                await _unitOfWork.CommitChangesAsync();
                return;
            }
            else
            {
                //TODO the attacker lost a portion of it's units
                //TODO the defender lost all of this phases's defending units               
            }
            
            //TODO repeate this process for a CavalryDefensePhase and an ArcherDefensePhase

        }


        private async Task RemoveTheFallenUnits(string barrackId, FallenUnits fallenUnits) 
        {
            IEnumerable<UnitsInCity> unitsInCities = await _unitOfWork.Units.GetUnitsInCityByBarrackId(barrackId);
            unitsInCities.ReduceAmount("Spearman",fallenUnits.Spearman);
            unitsInCities.ReduceAmount("Swordsman", fallenUnits.Swordsman);
            unitsInCities.ReduceAmount("Axe Fighter", fallenUnits.AxeFighter);
            unitsInCities.ReduceAmount("Archer", fallenUnits.Archer);
            unitsInCities.ReduceAmount("Light Cavalry", fallenUnits.LightCavalry);
            unitsInCities.ReduceAmount("Mounted Archer", fallenUnits.MountedArcher);
            unitsInCities.ReduceAmount("Heavy Cavalry", fallenUnits.HeavyCavalry);
        }

        


        private double GetInfantryDefenseTotal(IEnumerable<UnitsInCity> units, double percentage)
        {
            return 0 +
                CalculateInfantryDefense(units, "Spearman", percentage) +
                CalculateInfantryDefense(units, "Swordsman", percentage) +
                CalculateInfantryDefense(units, "Axe Fighter", percentage) +
                CalculateInfantryDefense(units, "Archer", percentage) +
                CalculateInfantryDefense(units, "Light Cavalry", percentage) +
                CalculateInfantryDefense(units, "Mounted Archer", percentage) +
                CalculateInfantryDefense(units, "Heavy Cavalry", percentage);
        }
                
                
        private double CalculateInfantryDefense(IEnumerable<UnitsInCity> units, string unitType, double percentage)
        {
            var unit = units.FirstOrDefault(u => u.Unit.Name.Equals(unitType));
            if (unit == null)
                return 0;
            else return unit.Amount * percentage * unit.Unit.InfantryDefensePoint;
        }


        private (int infantryTotalStrength, int cavalryTotalStrength, int archerTotalStrength) CalculateAttackerUnitTypeStrength(AttackRequest request)
        {
            //infantry strength
            int spearmanStrength = _unitOfWork.Units.FindUnitByName("Spearman").Result.AttackPoint * request.SpearmanAmount;
            int swordsmanStrength = _unitOfWork.Units.FindUnitByName("Swordsman").Result.AttackPoint * request.SwordsmanAmount;
            int axeFighterStrength = _unitOfWork.Units.FindUnitByName("Axe Fighter").Result.AttackPoint * request.AxeFighterAmount;
            int infantryTotal = spearmanStrength + swordsmanStrength + axeFighterStrength;

            //cavalry strength
            int lightCavalryStrength = _unitOfWork.Units.FindUnitByName("Light Cavalry").Result.AttackPoint * request.LightCavalryAmount;
            int heavyCavalryStrength = _unitOfWork.Units.FindUnitByName("Heavy Cavalry").Result.AttackPoint * request.HeavyCavalryAmount;
            int cavalryTotal = lightCavalryStrength + heavyCavalryStrength;
            //archer strength
            int archerStrength = _unitOfWork.Units.FindUnitByName("Archer").Result.AttackPoint * request.ArcherAmount;
            int mountedArcherStrength = _unitOfWork.Units.FindUnitByName("Mounted Archer").Result.AttackPoint * request.MountedArcherAmount;
            int archerTotal = archerStrength + mountedArcherStrength;

            return (infantryTotal, cavalryTotal, archerTotal);
        }
    }
    public static class extension 
    {
        public static void ReduceAmount(this IEnumerable<UnitsInCity> unitsInCity, string unitName, int removeAmount)
        {
            unitsInCity.FirstOrDefault(u => u.Unit.Name.Equals(unitName)).Amount -= removeAmount;
        }
    }
    public class FallenUnits
    {
        public int Spearman { get; set; }
        public int Swordsman { get; set; }
        public int AxeFighter { get; set; }
        public int LightCavalry { get; set; }
        public int HeavyCavalry { get; set; }
        public int Archer { get; set; }
        public int MountedArcher { get; set; }
    }
}
