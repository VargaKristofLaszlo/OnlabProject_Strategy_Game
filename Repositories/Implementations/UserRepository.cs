using BackEnd.Models.Models;
using BackEnd.Models.Response;
using BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Implementations
{
    public class UserRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ApplicationUser> FindUserByIdOrNullAsync(string id)
            => await _userManager.FindByIdAsync(id);

        public async Task<ApplicationUser> FindUserByEmailOrNullAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<ApplicationUser> FindUserByUsernameOrNullAsync(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<string> FindUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }

        public async Task<(IEnumerable<ApplicationUser> Users, int Count)> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            return (await _db.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(), _db.Users.ToListAsync().Result.Count);
        }

        public async Task<ApplicationUser> GetUserWithCities(string userId)
        {
            return await _db.Users
                .Include(user => user.Cities)
                    .ThenInclude(city => city.Resources)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.CityWall)
                        .ThenInclude(building => building.UpgradeCost)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.Farm)
                        .ThenInclude(building => building.UpgradeCost)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.Warehouse)
                        .ThenInclude(building => building.UpgradeCost)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.StoneProduction)
                        .ThenInclude(building => building.UpgradeCost)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.SilverProduction)
                        .ThenInclude(building => building.UpgradeCost)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.WoodProduction)
                        .ThenInclude(building => building.UpgradeCost)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.Castle)
                        .ThenInclude(building => building.UpgradeCost)
                .Include(user => user.Cities)
                    .ThenInclude(city => city.Tavern)
                        .ThenInclude(building => building.UpgradeCost)
                .FirstOrDefaultAsync(user => user.Id.Equals(userId));
        }

        public async Task<UsermanagerResponse> CreateUserAsync(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);

                return UsermanagerResponse.TaskCompletedSuccessfully();
            }

            IEnumerable<string> errors = result.Errors.Select(error => error.Description);
            return UsermanagerResponse.TaskFailed(errors);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);

            return WebEncoders.Base64UrlEncode(encodedEmailToken);
        }

        public async Task<UsermanagerResponse> DeleteUserAsync(string userId)
        {
            var user = await GetUserWithCities(userId);
            if (user == null)
                return UsermanagerResponse.TaskCompletedSuccessfully();


            List<Barrack> barracks = new List<Barrack>();
            List<CityHall> cityHalls = new List<CityHall>();
            List<CityWall> cityWalls = new List<CityWall>();
            List<Farm> farms = new List<Farm>();
            List<ResourceProduction> resourceProductions = new List<ResourceProduction>();
            List<Warehouse> warehouses = new List<Warehouse>();
            List<UnitsInCity> unitsInCities = new List<UnitsInCity>();
            List<Castle> castles = new List<Castle>();
            List<Tavern> taverns = new List<Tavern>();

            foreach (var city in user.Cities)
            {
                barracks.Add(_db.Barracks.FirstOrDefault(x => x.CityId.Equals(city.Id)));
                cityHalls.Add(_db.CityHalls.FirstOrDefault(x => x.CityId.Equals(city.Id)));
                cityWalls.Add(_db.CityWalls.FirstOrDefault(x => x.CityId.Equals(city.Id)));
                farms.Add(_db.Farms.FirstOrDefault(x => x.CityId.Equals(city.Id)));
                resourceProductions.AddRange(_db.ResourceProductions.Where(x => x.CityId.Equals(city.Id)).ToList());
                warehouses.Add(_db.Warehouses.FirstOrDefault(x => x.CityId.Equals(city.Id)));
                castles.Add(_db.Castles.FirstOrDefault(x => x.CityId.Equals(city.Id)));
                taverns.Add(_db.Taverns.FirstOrDefault(x => x.CityId.Equals(city.Id)));
            }
            foreach (var barrack in barracks)
            {
                foreach (var units in barrack.UnitsInCity)
                {
                    unitsInCities.Add(units);
                }
            }

            var result = await _userManager.DeleteAsync(user);
            foreach (var item in barracks)
                _db.Barracks.Remove(item);

            foreach (var item in cityHalls)
                _db.CityHalls.Remove(item);

            foreach (var item in cityWalls)
                _db.CityWalls.Remove(item);

            foreach (var item in farms)
                _db.Farms.Remove(item);

            foreach (var item in resourceProductions)
                _db.ResourceProductions.Remove(item);

            foreach (var item in warehouses)
                _db.Warehouses.Remove(item);

            foreach (var item in castles)
                _db.Castles.Remove(item);

            foreach (var item in taverns)
                _db.Taverns.Remove(item);

            foreach (var item in unitsInCities)
            {
                _db.UnitsInCities.Remove(item);
            }

            await _db.SaveChangesAsync();

            if (result.Succeeded)
                return UsermanagerResponse.TaskCompletedSuccessfully();

            IEnumerable<string> errors = result.Errors.Select(error => error.Description);

            return UsermanagerResponse.TaskFailed(errors);
        }


        public async Task<(List<ApplicationUser> Users, int Count)> GetAllUsersWithCities(int pageNumber, int pageSize, string senderId)
        {
            return (await _db.Users
                .Where(x => x.Id.Equals(senderId) == false)
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .Include(user => user.Cities)
                .ToListAsync(), _db.Users.ToListAsync().Result.Count);
        }
    }
}
