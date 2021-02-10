using BackEnd.Models.Models;
using BackEnd.Models.Response;
using BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Implementations
{
    public class UserRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ApplicationUser> FindUserByIdOrNullAsync(string id)
            => await _userManager.FindByIdAsync(id);

        public async Task<ApplicationUser> FindUserByEmailOrNullAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<ApplicationUser> FindUserByUsernameOrNullAsync(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<string> FindUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            return  await _db.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        public async Task<UsermanagerResponse> UpdateEmailAddressAsync(ApplicationUser user, string token, string newEmailAddress)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ChangeEmailAsync(user, newEmailAddress, normalToken);

            if (result.Succeeded)
                return UsermanagerResponse.TaskCompletedSuccessfully();

            IEnumerable<string> Errors = result.Errors.Select(error => error.Description);

            return UsermanagerResponse.TaskFailed(Errors);
        }

        public async Task<UsermanagerResponse> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
                return UsermanagerResponse.TaskCompletedSuccessfully();

            IEnumerable<string> Errors = result.Errors.Select(error => error.Description);

            return UsermanagerResponse.TaskFailed(Errors);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);

            return WebEncoders.Base64UrlEncode(encodedEmailToken);
        }

        public async Task<string> GeneratePasswordResetTokenAsnyc(ApplicationUser user)
        {
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedPasswordResetToken = Encoding.UTF8.GetBytes(passwordResetToken);

            return WebEncoders.Base64UrlEncode(encodedPasswordResetToken);
        }

        public async Task<string> GenerateEmailUpdateToken(ApplicationUser user, string newEmail)
        {
            var emaiUpdateToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            var encodedEmailUpdateToken = Encoding.UTF8.GetBytes(emaiUpdateToken);

            return WebEncoders.Base64UrlEncode(encodedEmailUpdateToken);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<UsermanagerResponse> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return UsermanagerResponse.TaskCompletedSuccessfully();

            IEnumerable<string> errors = result.Errors.Select(error => error.Description);

            return UsermanagerResponse.TaskFailed(errors);
        }

        public async Task<UsermanagerResponse> ConfirmEmailUpdateAsync(ApplicationUser user, string token, string newEmail)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

            if (result.Succeeded)
                return UsermanagerResponse.TaskCompletedSuccessfully();

            IEnumerable<string> errors = result.Errors.Select(error => error.Description);

            return UsermanagerResponse.TaskFailed(errors);
        }

        public async Task<UsermanagerResponse> CreateUserAsync(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);

                return UsermanagerResponse.TaskCompletedSuccessfully();
            }

            IEnumerable<string> errors = result.Errors.Select(error => error.Description);
            return UsermanagerResponse.TaskFailed(errors);
        }


        public async Task<UsermanagerResponse> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, newPassword);

            if (result.Succeeded)
                return UsermanagerResponse.TaskCompletedSuccessfully();

            IEnumerable<string> errors = result.Errors.Select(error => error.Description);

            return UsermanagerResponse.TaskFailed(errors);
        }

        public async Task<UsermanagerResponse> DeleteUserAsync(ApplicationUser user) 
        {
            var result = await  _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return UsermanagerResponse.TaskCompletedSuccessfully();

            IEnumerable<string> errors = result.Errors.Select(error => error.Description);

            return UsermanagerResponse.TaskFailed(errors);
        }
    }
}
