using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Implementations
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private IUsersRepository _users;
        private IUpgradeCostRepository _upgradeCosts;

        public EfUnitOfWork(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
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

        public async Task CommitChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
