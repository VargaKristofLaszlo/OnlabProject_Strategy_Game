using BackEnd.Models.Models;
using BackEnd.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        //Query methods
        Task<ApplicationUser> FindUserByIdOrNullAsync(string id);
        Task<ApplicationUser> FindUserByEmailOrNullAsync(string email);
        Task<ApplicationUser> FindUserByUsernameOrNullAsync(string username);
        Task<string> FindUserRoleAsync(ApplicationUser user);
        Task<(IEnumerable<ApplicationUser> Users, int Count)> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<ApplicationUser> GetUserWithCities(string userId);

        //Token generation methods
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        //User creation method
        Task<UsermanagerResponse> CreateUserAsync(ApplicationUser user, string password, string role);

        //Delete method
        Task<UsermanagerResponse> DeleteUserAsync(string userId);
    }
}
