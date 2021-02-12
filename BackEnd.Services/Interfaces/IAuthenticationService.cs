using Shared.Models.Request;
using Shared.Models.Response;
using System.Threading.Tasks;

namespace BackEnd.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(UserLoginRequest request);
        Task RegisterUserAsync(UserCreationRequest request);        
        Task ConfirmEmailAsync(string userId, string token);
        Task ForgetPasswordAsync(string email);
        Task ResetPasswordAsync(PasswordResetRequest resetRequest);
        Task UpdatePasswordAsync(string currentPassword, string newPassword);
        Task SendEmailUpdateConfirmationAsync(string newEmailAddress);
        Task UpdateEmailAddressAsync(string username, string newEmail, string token);
        Task DeleteAccountAsync(string username);
        Task SendAccountDeletionConfirmationAsync();
    }
}
