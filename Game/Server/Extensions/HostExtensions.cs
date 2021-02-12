using BackEnd.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.DataSeeding;
using System.Threading.Tasks;

namespace Game.Server.Extensions
{
    public static class HostExtensions
    {
        public async static Task<IHost> SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<ApplicationDbContext>();
                var userManager = services.GetService<UserManager<ApplicationUser>>();
                var roleManager = services.GetService<RoleManager<IdentityRole>>();

                await new UnitTypeSeeding(dbContext).SeedUnitTypes();
                await new BuildingUpgradeCostSeeding(dbContext).SeedBuildingUpgradeCost();
                await new UserSeeding(userManager, roleManager).SeedUserData();
            }

            return host;
        }
    }
}
