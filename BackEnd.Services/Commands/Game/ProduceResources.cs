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
    public class ProduceResources
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProduceResources(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private static void IncreaseCityResources(City city)
        {
            int woodAfterIncrease = city.Resources.Wood + city.WoodProduction.ProductionAmount;
            int stoneAfterIncrease = city.Resources.Stone + city.StoneProduction.ProductionAmount;
            int silverAfterIncrease = city.Resources.Silver + city.SilverProduction.ProductionAmount;

            city.Resources.Wood = Math.Min(woodAfterIncrease, city.Warehouse.MaxWoodStorageCapacity);
            city.Resources.Stone = Math.Min(stoneAfterIncrease, city.Warehouse.MaxStoneStorageCapacity);
            city.Resources.Silver = Math.Min(silverAfterIncrease, city.Warehouse.MaxSilverStorageCapacity);
        }

        public async Task Produce()
        {
            var cities = await _unitOfWork.Cities.GetAllCities();

            foreach (var city in cities)
            {
                IncreaseCityResources(city);
            }
            await _unitOfWork.CommitChangesAsync();
        }
    }
}

