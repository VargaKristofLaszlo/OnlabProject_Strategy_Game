using BackEnd.Models.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _db;

        public UnitRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> FindUnitByName(string name)
        {
            return await _db.Units.FirstOrDefaultAsync(unit => unit.Name.Equals(name));
        }

        public async Task<(IEnumerable<Unit> Units, int Count)> GetAllUnitsAsync(int pageNumber, int pageSize)
        {
            return (await _db.Units
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(), _db.Units.ToListAsync().Result.Count());
        }
        public async Task<IEnumerable<Unit>> GetAllUnitsAsync() 
        {
            return await _db.Units
                .ToListAsync();
        }


        public async Task<IEnumerable<Unit>> GetProducibleUnitTypes(int stage)
        {
            return await _db.Units
                .Include(unit => unit.UnitCost)
                .Where(type => type.MinBarrackStage <= stage)
                .ToListAsync();
        }

        public async Task<IEnumerable<UnitsInCity>> GetUnitsInCityByBarrackId(string barrackId)
        {
            return await _db.UnitsInCities
                .Include(u => u.Unit)
                .Where(u => u.BarrackId.Equals(barrackId))
                .ToListAsync();
        }

        public async Task<UnitsInCity> GetUnitsInCity(string unitId, string barrackId)
        {
            return await _db.UnitsInCities
                .Where(entity => entity.UnitId.Equals(unitId) && entity.BarrackId.Equals(barrackId))
                .FirstOrDefaultAsync();
        }




        public async Task InsertNewEntryToUnitsInCity(UnitsInCity unitsInCity) 
        {
            await _db.UnitsInCities.AddAsync(unitsInCity);
        }
    }
}
