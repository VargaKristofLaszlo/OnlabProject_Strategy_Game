using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Repositories.Implementations;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Implementations
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private IUsersRepository _users;
        private IUpgradeCostRepository _upgradeCosts;
        private IUnitRepository _units;
        private ICityRepository _cities;
        private IReportRepository _reports;
        private IHangFireRepository _hangFire;

        public EfUnitOfWork(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IHangFireRepository HangFire 
        {
            get 
            {
                if (_hangFire == null)
                    _hangFire = new HangFireRepository(_db);
                return _hangFire;
            }
        }

        public IReportRepository Reports
        {
            get
            {
                if (_reports == null)
                    _reports = new ReportRepository(_db);

                return _reports;
            }
        }

        public IUsersRepository Users
        {
            get
            {
                if (_users == null)
                    _users = new UserRepository(_userManager, _db);

                return _users;
            }
        }

        public IUpgradeCostRepository UpgradeCosts
        {
            get
            {
                if (_upgradeCosts == null)
                    _upgradeCosts = new UpgradeCostRepository(_db);

                return _upgradeCosts;
            }
        }

        public IUnitRepository Units
        {
            get
            {
                if (_units == null)
                    _units = new UnitRepository(_db);

                return _units;
            }
        }

        public ICityRepository Cities 
        {
            get 
            {
                if (_cities == null)
                    _cities = new CityRepository(_db);
                return _cities;
            }
        }

        public async Task<int> CommitChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
