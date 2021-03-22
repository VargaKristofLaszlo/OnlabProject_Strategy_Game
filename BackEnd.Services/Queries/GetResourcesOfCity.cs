using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using Services.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetResourcesOfCity
    {
        public record Query(int CityIndex) : IRequest<CityResources>;

        public class Handler : IRequestHandler<Query, CityResources>
        {
            private readonly IUnitOfWork _unitOfWork;          
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityContext;
            }

            public async Task<CityResources> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);

                if (user == null)
                    throw new NotFoundException();

                var city = await _unitOfWork.Cities.FindCityById(user.Cities[request.CityIndex].Id);

                int hourDifference = (int)(DateTime.UtcNow - city.LastResourceQueryTime).TotalHours;

                int stoneAmount = city.Resources.Stone;
                int silverAmount = city.Resources.Silver;
                int woodAmount = city.Resources.Wood;

                if (hourDifference > 0)
                {
                    //introducing local variables since we need to use ref
                    stoneAmount = city.Resources.Stone + city.StoneProduction.ProductionAmount * hourDifference;
                    silverAmount = city.Resources.Silver + city.SilverProduction.ProductionAmount * hourDifference;
                    woodAmount = city.Resources.Wood + city.WoodProduction.ProductionAmount * hourDifference;
                    city.LastResourceQueryTime = DateTime.UtcNow;

                   // CheckWarehouseCapacity(ref stoneAmount, ref silverAmount, ref woodAmount, city.Warehouse);

                    //updating the resources if the city with the storable amount
                    city.Resources.Stone = stoneAmount;
                    city.Resources.Silver = silverAmount;
                    city.Resources.Wood = woodAmount;

                    await _unitOfWork.CommitChangesAsync();
                }
                return new CityResources
                {
                    StoneAmount = stoneAmount,
                    StoneProductionPerHour = city.StoneProduction.ProductionAmount,
                    SilverAmount = silverAmount,
                    SilverProductionPerHour = city.SilverProduction.ProductionAmount,
                    WoodAmount = woodAmount,
                    WoodProductionPerHour = city.WoodProduction.ProductionAmount,
                    TotalPopulation = city.Farm.MaxPopulation,
                    FreePopulation = city.Resources.Population
                };
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
}
