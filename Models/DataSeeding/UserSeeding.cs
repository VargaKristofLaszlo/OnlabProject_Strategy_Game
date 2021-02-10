using BackEnd.Models.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BackEnd.Models.DataSeeding
{
    public class UserSeeding
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserSeeding(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedData() 
        {
            if (await _roleManager.FindByNameAsync("Admin") != null)
                return;

            //Create role
            var adminRole = new IdentityRole { Name = "Admin" };
            await _roleManager.CreateAsync(adminRole);

            var userRole = new IdentityRole { Name = "User" };
            await _roleManager.CreateAsync(userRole);

            //Create User
            var admin = new ApplicationUser
            {
                Email = "test@gmail.com",
                UserName = "test",
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(admin, "Test.123");

            await _userManager.AddToRoleAsync(admin, "Admin");

        }
    }
}
