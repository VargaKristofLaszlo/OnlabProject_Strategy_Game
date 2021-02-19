using BackEnd.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUnitRepository
    {
        Task<Unit> FindUnitByName(string name);
        Task<(IEnumerable<Unit> Units, int Count)> GetAllUnitsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Unit>> GetAllUnitsAsync();
        Task<IEnumerable<Unit>> GetProducibleUnitTypes(int stage);
        Task<UnitsInCity> GetUnitsInCityByUnitId(string unitId);
        Task<IEnumerable<UnitsInCity>> GetUnitsInCityByBarrackId(string barrackId);
        Task InsertNewEntryToUnitsInCity(UnitsInCity unitsInCity);
    }
}
