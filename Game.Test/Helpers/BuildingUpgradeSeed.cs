using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Models.DataSeeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Test.Helpers
{
    public class BuildingUpgradeSeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public BuildingUpgradeSeed(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public IDbContextTransaction Transaction { get; set; }

        public async Task InitializeDbForTestsAsync(ApplicationDbContext db)
        {
            Transaction = db.Database.BeginTransaction();

            await new BuildingUpgradeCostSeeding(db).SeedBuildingUpgradeCost();
            await SeedUserAsync(db);

        }

        public async Task RollBack() => await Transaction.RollbackAsync();

        public async Task ReinitializeDbForTestsAsync(ApplicationDbContext db)
        {
            Transaction.Rollback();
            await InitializeDbForTestsAsync(db);
            await SeedUserAsync(db);
        }

        public async Task SeedUserAsync(ApplicationDbContext db)
        {
            if (await _roleManager.RoleExistsAsync("User") == false)
            {
                var userRole = new IdentityRole { Name = "User", NormalizedName = "USER" };
                await db.Roles.AddAsync(userRole);
            }




            //Create User
            var user = new ApplicationUser
            {
                Id = "test_id",
                Email = "test_user@gmail.com",
                UserName = "user",
                EmailConfirmed = true,
            };

            await db.Users.AddAsync(user);

            //       await _userManager.AddToRoleAsync(user, "User");

            //Get the upgrade costs which will be used to create the buildings
            BuildingUpgradeCost warehouseCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Warehouse", 1);
            BuildingUpgradeCost silverMineCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("SilverMine", 1);
            BuildingUpgradeCost stoneMineCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("StoneMine", 1);
            BuildingUpgradeCost lumberCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Lumber", 1);
            BuildingUpgradeCost farmCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Farm", 1);
            BuildingUpgradeCost cityWallCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("CityWall", 1);
            BuildingUpgradeCost cityhallCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("CityHall", 1);
            BuildingUpgradeCost barrackCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Barrack", 1);
            BuildingUpgradeCost castleCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Castle", 1);
            BuildingUpgradeCost tavernCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Tavern", 1);

            //Create the buildings
            Warehouse warehouse = Warehouse.Create(warehouseCost);
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
            BackEnd.Models.Models.Castle castle = BackEnd.Models.Models.Castle.Create(castleCost);
            Tavern tavern = Tavern.Create(tavernCost);

            //Add the buildings to the city
            City city = new City
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
                Loyalty = 100
            };

            user.Cities = new List<City>() { city };

            await db.SaveChangesAsync();
            ;
        }
    }
}
