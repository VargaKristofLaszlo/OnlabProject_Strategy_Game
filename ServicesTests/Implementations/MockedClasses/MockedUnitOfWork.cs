using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServicesTests.Implementations.MockedClasses
{
    public class MockedUnitOfWork : IUnitOfWork
    {
        private readonly string attackingUserId;
        private readonly string defendingUserId;
        private readonly string attackingBarrackId;
        private readonly string defendingBarrackId;
        private IUsersRepository _users;
        private IUpgradeCostRepository _upgradeCosts;
        private IUnitRepository _units;
        private ICityRepository _cities;

        public MockedUnitOfWork(string attackingUserId, string defendingUserId, string attackingBarrackId, string defendingBarrackId)
        {
            this.attackingUserId = attackingUserId;
            this.defendingUserId = defendingUserId;
            this.attackingBarrackId = attackingBarrackId;
            this.defendingBarrackId = defendingBarrackId;
        }
        public IUsersRepository Users
        {
            get
            {
                if (_users == null)
                    _users = new MockedUserRepository(attackingUserId, defendingUserId, attackingBarrackId, defendingBarrackId);
                return _users;
            }
        }

        public IUpgradeCostRepository UpgradeCosts
        {
            get
            {
                if (_upgradeCosts == null)
                    _upgradeCosts = new MockedUpgradeCostRepository();
                return _upgradeCosts;
            }
        }

        public IUnitRepository Units
        {
            get
            {
                if (_units == null)
                    _units = new MockedUnitRepository(attackingBarrackId, defendingBarrackId);
                return _units;
            }
        }

        public ICityRepository Cities {
            get
            {
                if (_cities == null)
                    _cities = new MockedCityRepository();
                return _cities;
            }
        }

        public async Task CommitChangesAsync()
        {
            return;
        }
        
    }
}
