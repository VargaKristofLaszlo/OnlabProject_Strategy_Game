using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.Implementations.MockedClasses
{
    public class MockedUpgradeCostRepository : IUpgradeCostRepository
    {
        public Task CreateAsync(BuildingUpgradeCost upgradeCost)
        {
            throw new NotImplementedException();
        }

        public Task<int?> FindMaxStage(string buildingName)
        {
            throw new NotImplementedException();
        }

        public Task<BuildingUpgradeCost> FindUpgradeCost(string buildingName, int buildingStage)
        {
            throw new NotImplementedException();
        }
    }
}
