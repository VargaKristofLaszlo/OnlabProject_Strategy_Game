using BackEnd.Models.Models;
using Moq;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.Implementations.MockedClasses
{
    public class MockedUnitRepository : IUnitRepository
    {
        public Mock<UnitsInCity> attackingUnits = new Mock<UnitsInCity>().SetupAllProperties();
        public Mock<UnitsInCity> defendingUnits = new Mock<UnitsInCity>().SetupAllProperties();

        public MockedUnitRepository(string attackingBarrackId, string defendingBarrackId)
        {

         //   attackingUnits.SetupGet(x => x.BarrackId).Returns(attackingBarrackId);
         //   defendingUnits.SetupGet(x => x.BarrackId).Returns(defendingBarrackId);
        }



        public async Task<IEnumerable<UnitsInCity>> GetUnitsInCityByBarrackId(string barrackId)
        {
            if (attackingUnits.Object.BarrackId.Equals(barrackId))
                return new List<UnitsInCity>() { attackingUnits.Object };

            else if (defendingUnits.Object.BarrackId.Equals(barrackId))
                return new List<UnitsInCity>() { defendingUnits.Object };

            return null;
        }


        public Task<Unit> FindUnitByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<Unit> Units, int Count)> GetAllUnitsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Unit>> GetProducibleUnitTypes(int stage)
        {
            throw new NotImplementedException();
        }

        public Task<UnitsInCity> GetUnitsInCity(string unitId, string barrackId)
        {
            throw new NotImplementedException();
        }

        public Task InsertNewEntryToUnitsInCity(UnitsInCity unitsInCity)
        {
            throw new NotImplementedException();
        }
    }
}
