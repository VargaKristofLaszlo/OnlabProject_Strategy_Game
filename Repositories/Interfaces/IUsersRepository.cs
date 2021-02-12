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

        //Update methods      
        Task<UsermanagerResponse> UpdateEmailAddressAsync(ApplicationUser user, string token, string newEmailAddress);
        Task<UsermanagerResponse> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<UsermanagerResponse> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        //Token generation methods
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<string> GeneratePasswordResetTokenAsnyc(ApplicationUser user);
        Task<string> GenerateEmailUpdateToken(ApplicationUser user, string newEmail);

        //Confirmation methods
        Task<UsermanagerResponse> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<UsermanagerResponse> ConfirmEmailUpdateAsync(ApplicationUser user, string token, string newEmail);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        //User creation method
        Task<UsermanagerResponse> CreateUserAsync(ApplicationUser user, string password, string role);

        //Delete method
        Task<UsermanagerResponse> DeleteUserAsync(ApplicationUser user);

    }
}
