using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BackEnd.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using BackEnd.Repositories.Interfaces;

namespace Game.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                City city = await CreateCity(user);
                user.Cities.Add(city);
                await _unitOfWork.CommitChangesAsync();
            }
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }


        private async Task<City> CreateCity(ApplicationUser user)
        {
            //Get the upgrade costs which will be used to create the buildings
            BuildingUpgradeCost warehouseCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Warehouse", 1);
            BuildingUpgradeCost silverMineCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("SilverMine", 1);
            BuildingUpgradeCost stoneMineCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("StoneMine", 1);
            BuildingUpgradeCost lumberCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Lumber", 1);
            BuildingUpgradeCost farmCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Farm", 1);
            BuildingUpgradeCost cityWallCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("CityWall", 1);
            BuildingUpgradeCost cityhallCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("CityHall", 1);
            BuildingUpgradeCost barrackCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Barrack", 1);

            //Create the buildings
            Warehouse warehouse = Warehouse.Create(warehouseCost);
            ResourceProduction silverMine = ResourceProduction.CreateResourceProductionBuilding(silverMineCost);
            ResourceProduction stoneMine = ResourceProduction.CreateResourceProductionBuilding(stoneMineCost);
            ResourceProduction lumber = ResourceProduction.CreateResourceProductionBuilding(lumberCost);
            Farm farm = Farm.Create(farmCost);
            CityWall cityWall = CityWall.Create(cityWallCost);
            CityHall cityHall = CityHall.Create(cityhallCost);
            Barrack barrack = Barrack.Create(barrackCost);

            //Add the buildings to the city
            return new City
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
                Warehouse = warehouse
            };
        }
    }
}
