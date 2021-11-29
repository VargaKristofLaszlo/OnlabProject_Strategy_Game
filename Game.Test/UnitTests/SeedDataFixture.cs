using BackEnd.Models.Models;
using Game.Test.Data.Seed;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Game.Test.UnitTests
{
    public class SeedDataFixture : IDisposable
    {
        private readonly DataSeeder _dataSeeder;

        public ApplicationDbContext DbContext { get; private set; }

        public SeedDataFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var someOptions = Options.Create(new OperationalStoreOptions());

            DbContext = new ApplicationDbContext(options, someOptions);

            DbContext.SaveChanges();
            _dataSeeder = new DataSeeder(DbContext);
        }

        public void Dispose()
        {
            DbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SeedUsersAsync() => await _dataSeeder.SeedUsers();

        public async Task SeedBuildingUpgradeCostsAsync() => await _dataSeeder.SeedBuildingCosts();

        public async Task AddCityToUsersAsync() => await _dataSeeder.AddCityToUsers();

        public async Task SeedUnitTypes() => await _dataSeeder.SeedUnits();

        public async Task AddAttackingForcesToCity(string userId)
        {
            var user = await DbContext.Users
                .Include(x => x.Cities)
                .FirstAsync(x => x.Id.Equals(userId));

            var lightCavalry = await DbContext.Units
                .FirstAsync(x => x.Name.Equals("Light Cavalry"));

            var noble = await DbContext.Units
                .FirstAsync(x => x.Name.Equals("Noble"));

            user.Cities[0].Barrack.UnitsInCity.Add(new UnitsInCity()
            {
                Amount = 100,
                Unit = lightCavalry,
            });
            user.Cities[0].Barrack.UnitsInCity.Add(new UnitsInCity()
            {
                Amount = 1,
                Unit = noble,
            });
        }
    }
}
