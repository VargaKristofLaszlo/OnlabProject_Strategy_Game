using Shared.Models.Request;
using Shared.Models.Response;
using System.Threading.Tasks;

namespace BackEnd.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(UserLoginRequest request);
        Task<OperationResponse> RegisterUserAsync(UserCreationRequest request);        
        Task<OperationResponse> ConfirmEmailAsync(string userId, string token);
        Task<OperationResponse> ForgetPasswordAsync(string email);
        Task<OperationResponse> ResetPasswordAsync(PasswordResetRequest resetRequest);
        Task<OperationResponse> UpdatePasswordAsync(string currentPassword, string newPassword);
        Task<OperationResponse> SendEmailUpdateConfirmationAsync(string newEmailAddress);
        Task<OperationResponse> UpdateEmailAddressAsync(string username, string newEmail, string token);
        Task<OperationResponse> DeleteAccountAsync(string username);
        Task<OperationResponse> SendAccountDeletionConfirmationAsync();
    }
}
