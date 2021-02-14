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
        Task<IEnumerable<Unit>> GetProducibleUnitTypes(int stage);
    }
}
