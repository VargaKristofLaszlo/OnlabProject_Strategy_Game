﻿using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Extensions;
using BackEnd.Services.Interfaces;
using Shared.Models;
using Shared.Models.Response;
using System.Linq;
using System.Threading.Tasks;
using Services.Exceptions;
using AutoMapper;
using Game.Shared.Models;
using BackEnd.Infrastructure;
using System.Collections.Generic;
using System;


namespace BackEnd.Services.Implementations
{
    public class ViewService : IViewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IdentityOptions _identityOptions;

        public ViewService(IUnitOfWork unitOfWork, IMapper mapper, IdentityOptions identityOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _identityOptions = identityOptions;
        }

        private int CalculatePageCount(int modelCount, int pageSize)
        {
            int pagesCount = modelCount / pageSize;

            if (modelCount % pageSize != 0)
                return pagesCount + 1;

            return pagesCount;
        }

        public async Task<BuildingUpgradeCost> GetBuildingUpgradeCost(string buildingName, int buildingStage)
        {
            var result = await _unitOfWork.UpgradeCosts.FindUpgradeCost(buildingName, buildingStage);

            if (result == null)
                throw new NotFoundException();

            return _mapper.Map<BuildingUpgradeCost>(result);
        }

        public async Task<CollectionResponse<string>> GetCityNamesOfUser(int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();

            var user = await _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId);

            var cityNameList = user.Cities.Select(city => city.CityName);
            int cityNameListCount = cityNameList.Count();

            var cityNameListForPage = cityNameList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new CollectionResponse<string>
            {
                Records = cityNameListForPage,
                PagingInformations = new PagingInformations
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = CalculatePageCount(cityNameListCount, pageSize)
                }
            };
        }


        public async Task<CollectionResponse<Credentials>> GetUserCredentialsAsync(int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();

            var credentialList = await _unitOfWork.Users.GetAllUsersAsync(pageNumber, pageSize);

            var credentialListForPage = credentialList.Users
                    .Select(model => _mapper.Map<Credentials>(model));

            return new CollectionResponse<Credentials>
            {
                Records = credentialListForPage,
                PagingInformations = new PagingInformations
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = CalculatePageCount(credentialList.Count, pageSize)
                }
            };
        }

        public async Task<CollectionResponse<Unit>> GetUnitTypes(int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();

            var unitList = await _unitOfWork.Units.GetAllUnitsAsync(pageNumber, pageSize);

            var unitListForPage = unitList.Units
                .Select(model => _mapper.Map<Unit>(model));

            return new CollectionResponse<Unit>
            {
                Records = unitListForPage,
                PagingInformations = new PagingInformations
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = CalculatePageCount(unitList.Count, pageSize)
                }
            };
        }

        private async Task<Models.Models.City> GetCityData(int cityIndex)
        {
            var user = await _unitOfWork.Users.GetUserWithCities(_identityOptions.UserId);

            if (user == null)
                throw new NotFoundException();

            var city = await _unitOfWork.Cities.FindCityById(user.Cities[cityIndex].Id);

            return city;
        }

        public async Task<CityDetails> GetCityDetails(int cityIndex)
        {
            return _mapper.Map<CityDetails>(await GetCityData(cityIndex));
        }


        public async Task<IEnumerable<Unit>> GetProducibleUnitTypes(int cityIndex)
        {
            CityDetails cityDetails = _mapper.Map<CityDetails>(await GetCityData(cityIndex));

            var producibleUnits = await _unitOfWork.Units.GetProducibleUnitTypes(cityDetails.BarrackStage);

            return producibleUnits.ToList().Select(unit => _mapper.Map<Unit>(unit));
        }

        public async Task<CityResources> GetResourcesOfCity(int cityIndex) 
        {
            var city = await GetCityData(cityIndex);

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

                CheckWarehouseCapacity(ref stoneAmount,ref silverAmount,ref woodAmount, city.Warehouse);

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
                WoodProductionPerHour = city.WoodProduction.ProductionAmount
            };
        }

        private void CheckWarehouseCapacity(ref int stoneAmount, ref int silverAmount, ref int woodAmount, Models.Models.Warehouse warehouse) 
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