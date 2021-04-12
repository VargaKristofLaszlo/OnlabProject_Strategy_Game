using BackEnd.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<City> FindCityById(string id);
        Task<Warehouse> FindWarehouseOfCity(int cityIndex, string userId);

        Task<List<City>> GetAllCities();
    }
}
