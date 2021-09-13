using BackEnd.Models.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _db;

        public CityRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<City> FindCityById(string id)
        {
            return await _db.Cities
                 .Include(city => city.Resources)
                 .Include(city => city.Barrack)
                     .ThenInclude(barrack => barrack.UpgradeCost)
                 .Include(city => city.CityHall)
                     .ThenInclude(cityHall => cityHall.UpgradeCost)
                 .Include(city => city.CityWall)
                     .ThenInclude(cityWall => cityWall.UpgradeCost)
                 .Include(city => city.Farm)
                     .ThenInclude(farm => farm.UpgradeCost)
                 .Include(city => city.SilverProduction)
                     .ThenInclude(silverProduction => silverProduction.UpgradeCost)
                 .Include(city => city.StoneProduction)
                     .ThenInclude(stoneProduction => stoneProduction.UpgradeCost)
                 .Include(city => city.WoodProduction)
                     .ThenInclude(woodProduction => woodProduction.UpgradeCost)
                 .Include(city => city.Warehouse)
                     .ThenInclude(warehouse => warehouse.UpgradeCost)
                  .Include(city => city.Castle)
                     .ThenInclude(castle => castle.UpgradeCost)
                .FirstOrDefaultAsync(city => city.Id.Equals(id));
        }



        public async Task<Warehouse> FindWarehouseOfCity(int cityIndex, string userId)
        {
            var cititesOfUser = await _db.Cities
                .Where(city => city.UserId.Equals(userId))
                .Include(city => city.Warehouse)
                .ToListAsync();

            return cititesOfUser.ElementAt(cityIndex).Warehouse;
        }

        public async Task<List<City>> GetAllCities()
        {
            return await _db.Cities
                .Include(city => city.StoneProduction)
                .Include(city => city.WoodProduction)
                .Include(city => city.SilverProduction)
                .Include(city => city.Warehouse)
                .Include(city => city.Resources)
                .ToListAsync();
        }
    }
}
