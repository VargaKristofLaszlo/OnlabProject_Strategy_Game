using BackEnd.Models.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.Implementations.MockedClasses
{
    public class MockedCityRepository : ICityRepository
    {
        public Task<City> FindCityById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
