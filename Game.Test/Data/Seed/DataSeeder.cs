using BackEnd.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.DataSeeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Test.Data.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public DataSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedUsers()
        {
            //Create User            
            await _dbContext.Users.AddAsync(new ApplicationUser
            {
                Email = "test@test.test",
                UserName = TestDataConstants.UserNameOne,
                EmailConfirmed = true,
                Id = TestDataConstants.UserIdOne
            });

            await _dbContext.Users.AddAsync(new ApplicationUser
            {
                Email = "test@test.test",
                UserName = TestDataConstants.UserNameTwo,
                EmailConfirmed = true,
                Id = TestDataConstants.UserIdTwo
            });

            await _dbContext.Users.AddAsync(new ApplicationUser
            {
                Email = "test@test.test",
                UserName = TestDataConstants.AdminName,
                EmailConfirmed = true,
                Id = TestDataConstants.AdminId
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedBuildingCosts()
        {
            var buildingUpgradeCostSeeder = new BuildingUpgradeCostSeeding(_dbContext);

            await buildingUpgradeCostSeeder.SeedBuildingUpgradeCost();

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCityToUsers()
        {

            var userOne = await _dbContext.Users
                .Include(x => x.Cities)
                .FirstAsync(x => x.Id.Equals(TestDataConstants.UserIdOne));

            var userTwo = await _dbContext.Users
                .Include(x => x.Cities)
                .FirstAsync(x => x.Id.Equals(TestDataConstants.UserIdTwo));

            userOne.Cities.Add(await CreateCity(userOne));
            userTwo.Cities.Add(await CreateCity(userTwo));

            await _dbContext.SaveChangesAsync();
        }

        private async Task<City> CreateCity(ApplicationUser user)
        {
            //Get the upgrade costs which will be used to create the buildings
            BuildingUpgradeCost warehouseCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("Warehouse") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost silverMineCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("SilverMine") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost stoneMineCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("StoneMine") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost lumberCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("Lumber") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost farmCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("Farm") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost cityWallCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("CityWall") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost cityhallCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("CityHall") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost barrackCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("Barrack") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost castleCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("Castle") && x.BuildingStage.Equals(1));

            BuildingUpgradeCost tavernCost = await _dbContext.BuildingUpgradeCosts
                .FirstAsync(x => x.BuildingName.Equals("Tavern") && x.BuildingStage.Equals(1));

            //Create the buildings
            Warehouse warehouse = Warehouse.Create(warehouseCost);
            warehouse.MaxSilverStorageCapacity = 9999;
            warehouse.MaxStoneStorageCapacity = 9999;
            warehouse.MaxWoodStorageCapacity = 9999;
            ResourceProduction silverMine = ResourceProduction.CreateResourceProductionBuilding(silverMineCost);
            silverMine.ResourceType = ResourceType.silver;
            ResourceProduction stoneMine = ResourceProduction.CreateResourceProductionBuilding(stoneMineCost);
            stoneMine.ResourceType = ResourceType.stone;
            ResourceProduction lumber = ResourceProduction.CreateResourceProductionBuilding(lumberCost);
            lumber.ResourceType = ResourceType.wood;
            Farm farm = Farm.Create(farmCost);
            CityWall cityWall = CityWall.Create(cityWallCost);
            CityHall cityHall = CityHall.Create(cityhallCost);
            Barrack barrack = Barrack.Create(barrackCost);

            if (user.Id.Equals(TestDataConstants.UserIdOne))
                barrack.Id = TestDataConstants.BarrackIdOne;
            else
                barrack.Id = TestDataConstants.BarrackIdTwo;

            BackEnd.Models.Models.Castle castle = BackEnd.Models.Models.Castle.Create(castleCost);
            Tavern tavern = Tavern.Create(tavernCost);

            //Add the buildings to the city
            return new City()
            {
                CityName = $"{user.UserName}'s city",
                Resources = new Resources
                {
                    Wood = 1000,
                    Stone = 1000,
                    Silver = 1000,
                    Population = 100
                },
                UserId = user.Id,
                User = user,
                SilverProductionId = silverMine.Id,
                SilverProduction = silverMine,
                StoneProductionId = stoneMine.Id,
                StoneProduction = stoneMine,
                WoodProductionId = lumber.Id,
                WoodProduction = lumber,
                BarrackId = barrack.Id,
                Barrack = barrack,
                FarmId = farm.Id,
                Farm = farm,
                CityWallId = cityWall.Id,
                CityWall = cityWall,
                CityHallId = cityHall.Id,
                CityHall = cityHall,
                WarehouseId = warehouse.Id,
                Warehouse = warehouse,
                Castle = castle,
                Tavern = tavern,
                Loyalty = 100,
                Id = user.Id
            };
        }

        public async Task SeedUnits()
        {
            var unitTypeSeeding = new UnitTypeSeeding(_dbContext);

            await unitTypeSeeding.SeedUnitTypes();

            await _dbContext.SaveChangesAsync();
        }
    }
}
