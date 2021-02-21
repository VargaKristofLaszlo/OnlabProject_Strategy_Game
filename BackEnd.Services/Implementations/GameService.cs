using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using Services.Exceptions;
using Services.Implementations.AttackPhaseBehaviourImpl;
using Services.Implementations.BuildingBehaviourImpl;
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

        /*
            The fight is divided into 3 phases:
                -infantry
                -cavalry
                -archers
            Each phase the attacker uses the unit types according to the phase.
                So in the infantry phase the attacker can only use infantry units.
            The defender can use all of it's unit types in every phase however the amount used is limited.
            The used amount is calculated via this formula:
               percentage = (sum of population cost of the unit type used in the current phase) / (sum of population cost of all the attacking units)
               For every defending unit type amount =  amount of units of this type * percentage
         */
        public async Task AttackOtherCity(AttackRequest request)
        {
            //Initialize the variables for the attack process
            var initValues = await InitAttackProcess(request);
            AttackingTroops attackingTroops = initValues.attackingTroops;
            DefendingTroops defendingTroops = initValues.defendingTroops;
            IEnumerable<UnitsInCity> unitsOfAttacker = initValues.unitsOfAttacker;
            IEnumerable<UnitsInCity> defendingUnits = initValues.defendingUnits;

            var infantryPhaseResult = InfantryPhase(attackingTroops, defendingTroops);

            //Add the survivors of the previous phase to the next one
            defendingTroops.AddSurvivorsOfPreviousPhase(infantryPhaseResult.defendingTroops, defendingTroops.CavalryPhaseDefendingUnits);
            attackingTroops.AddSurvivorsOfPreviousPhase(infantryPhaseResult.attackerTroops, attackingTroops.CavalryPhaseTroops);

            var cavalryPhaseResult = CavalryPhase(attackingTroops, defendingTroops);

            //Add the survivors of the previous phase to the next one
            defendingTroops.AddSurvivorsOfPreviousPhase(cavalryPhaseResult.defendingTroops, defendingTroops.ArcheryPhaseDefendingUnits);
            attackingTroops.AddSurvivorsOfPreviousPhase(cavalryPhaseResult.attackerTroops, attackingTroops.ArcheryPhaseTroops);
            
            var archeryPhaseResult = ArcheryPhase(attackingTroops, defendingTroops);

            //Update the attacking and defending side
            foreach (var item in archeryPhaseResult.attackerTroops)
                unitsOfAttacker.Where(u => u.Equals(item.Key)).First().Amount = item.Value;

            foreach (var item in archeryPhaseResult.defendingTroops)
                defendingUnits.Where(d => d.Unit.Equals(item.Key)).First().Amount = item.Value;            
        }

        private (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            InfantryPhase(AttackingTroops attackingTroops, DefendingTroops defendingTroops) 
        {
            var infantryPhaseBehaviour = new InfantryAttackPhaseBehaviour();
            return infantryPhaseBehaviour.Action(attackingTroops, defendingTroops);
        }

        private (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            CavalryPhase(AttackingTroops attackingTroops, DefendingTroops defendingTroops)
        {
            var calvalryPhaseBehaviour = new CavalryAttackPhaseBehaviour();
            return calvalryPhaseBehaviour.Action(attackingTroops, defendingTroops);
        }  
        
        private (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            ArcheryPhase(AttackingTroops attackingTroops, DefendingTroops defendingTroops)
        {
            var archeryPhaseBehaviour = new ArcheryAttackPhaseBehaviour();
            return archeryPhaseBehaviour.Action(attackingTroops, defendingTroops);
        }

        private async Task<(AttackingTroops attackingTroops, DefendingTroops defendingTroops, 
            IEnumerable<UnitsInCity> unitsOfAttacker, IEnumerable<UnitsInCity> defendingUnits)> InitAttackProcess(AttackRequest request) 
        {
            var attackingUser = await _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId);
            var defendingUser = await _unitOfWork.Users.GetUserWithCities(request.AttackedUsername);
            if (defendingUser == null || attackingUser == null)
                throw new NotFoundException();

            var attackingCity = attackingUser.Cities.ElementAt(request.AttackerCityIndex);
            if (attackingCity == null)
                throw new NotFoundException();

            var unitsOfAttacker = await _unitOfWork.Units.GetUnitsInCityByBarrackId(attackingCity.Barrack.Id);

            //Convert the dto into a model in order to use it for the attack calculations
            Dictionary<Unit, int> attackingForces = new Dictionary<Unit, int>();
            foreach (var item in request.AttackingForces)
                attackingForces.Add(_mapper.Map<Unit>(item.Key), item.Value);


            AttackingTroops attackingTroops = new AttackingTroops(attackingForces);

            //Get the defending units
            var defendingUnits =
                await _unitOfWork.Units.GetUnitsInCityByBarrackId(defendingUser.Cities.ElementAt(request.AttackedCityIndex).Barrack.Id);

            DefendingTroops defendingTroops = new DefendingTroops(defendingUnits, attackingTroops.InfantryProvisionPercentage,
                attackingTroops.CavalryProvisionPercentage, attackingTroops.ArcheryProvisionPercentage);

            return (attackingTroops, defendingTroops,unitsOfAttacker,defendingUnits);
        }
    }
}
