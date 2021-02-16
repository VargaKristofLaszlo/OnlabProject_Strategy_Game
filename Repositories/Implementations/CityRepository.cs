using BackEnd.Models.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
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
           return await  _db.Cities
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
                .FirstOrDefaultAsync(city => city.Id.Equals(id));
        }
    }
}
