using Shared.Models.Request;
using Shared.Models.Response;
using System.Threading.Tasks;

namespace BackEnd.Services.Interfaces
{
    public interface IAuthenticationService
    {
        
        Task UpdatePasswordAsync(string currentPassword, string newPassword);
        Task SendEmailUpdateConfirmationAsync(string newEmailAddress);
        Task UpdateEmailAddressAsync(string username, string newEmail, string token);
        Task DeleteAccountAsync(string username, string token);
        Task SendAccountDeletionConfirmationAsync();
    }
}
