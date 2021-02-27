using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using Services.Exceptions;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityContext _identityContext;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IIdentityContext identityOptions, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identityContext = identityOptions;
            _mapper = mapper;
        }

        

        public async Task ProduceUnits(UnitProductionRequest request)
        {
            var city = await GetCityByCityIndex(request.CityIndex, _identityContext.UserId);

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

            var unitsOfThisTypeInCity = await _unitOfWork.Units.GetUnitsInCity(unitCheck.Type.Id, city.Barrack.Id);

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
            var senderCity = await GetCityByCityIndex(request.FromCityIndex, _identityContext.UserId);

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

        
    }
}
