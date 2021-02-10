using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories;
using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Interfaces;
using Shared.Models.Request;
using Shared.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackEnd.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthOptions _authOptions;
        private readonly IMailService _mailService;
        private readonly IdentityOptions _identity;

        public AuthenticationService(IUnitOfWork unitOfWork, AuthOptions authOptions,
            IMailService mailService, IdentityOptions identity)
        {
            _unitOfWork = unitOfWork;
            _authOptions = authOptions;
            _mailService = mailService;
            _identity = identity;
        }

        public async Task<OperationResponse> RegisterUserAsync(UserCreationRequest request)
        {
            var user = new ApplicationUser()
            {
                UserName = request.Username,
                Email = request.Email
            };

            var userManagerResponse =  await _unitOfWork.Users.CreateUserAsync(user, request.Password, "User");

            if (userManagerResponse.IsSuccess) 
            {  
                var token = await _unitOfWork.Users.GenerateEmailConfirmationTokenAsync(user);

                string url = $"{_authOptions.AppUrl}/api/Auth/ConfirmEmail?userid={user.Id}&token={token}";

                _mailService.SendEmail(user.Email, "Confirm your email", $"<h1>Welcome to my strategy game!</h1>" +
               $" <p>Please confirm your email by clicking <a href={url}>here</a></p>");

                return OperationResponse.Succeeded("Welcome to strategy game");
            }

            return OperationResponse.Failed("Registration failed", userManagerResponse.Errors);
        }

        public async Task<LoginResponse> LoginAsync(UserLoginRequest request)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(request.Username);

            if (user == null || await _unitOfWork.Users.CheckPasswordAsync(user, request.Password) == false)
                return LoginResponse.Failed("Invalid username or password");

            if (user.EmailConfirmed == false)
                return LoginResponse.Failed("Please confirm your email address first");

            if (user.IsBanned)
                return LoginResponse.Failed("Your account has been banned");
            

            var userRole = await _unitOfWork.Users.FindUserRoleAsync(user);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRole)
            };

            JwtFactory tokenFactory = new JwtFactory(_authOptions.Issuer, _authOptions.Audience, _authOptions.Key);

            JwtSecurityToken jwtToken = tokenFactory.GenerateToken(claims);

            return LoginResponse.Succeeded("Login was successful", tokenFactory.GenerateTokenString(jwtToken), jwtToken.ValidTo);
        }
        

        public async Task<OperationResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(userId);

            if (user == null)
                return OperationResponse.Failed("User was not found");

            var usermanagerResponse =  await _unitOfWork.Users.ConfirmEmailAsync(user, token);

            if(usermanagerResponse.IsSuccess)
                return OperationResponse.Succeeded("Your account confirmation is completed");

            return OperationResponse.Failed("Your account confirmation failed", usermanagerResponse.Errors);
        }

        public async Task<OperationResponse> ForgetPasswordAsync(string email)
        {
            var user = await _unitOfWork.Users.FindUserByEmailOrNullAsync(email);

            if (user == null)               
                return OperationResponse.Failed("There is no user registered with this email address");

            string token = await _unitOfWork.Users.GeneratePasswordResetTokenAsnyc(user);

            string url = $"{_authOptions.AppUrl}/ResetPassword?email={email}&token={token}";

            _mailService.SendEmail(email, "Reset password", $"<h1>Follow the instructions to reset your password</h1>" +
                    $"<p>To reset your password click <a href={url}>here</a> </p>");

            return OperationResponse.Succeeded("The password reset token was succesfully sent to the user's email address");          
        }

        public async Task<OperationResponse> ResetPasswordAsync(PasswordResetRequest resetRequest)
        {
            var user =  await _unitOfWork.Users.FindUserByEmailOrNullAsync(resetRequest.Email);

            if (user == null)
                return OperationResponse.Failed("No user associated with this email address");  

            var usermanagerResponse =  await _unitOfWork.Users.ResetPasswordAsync(user, resetRequest.Token, resetRequest.NewPassword);

            if (usermanagerResponse.IsSuccess)
                return OperationResponse.Succeeded("Password has been reseted");

            return OperationResponse.Failed("Invalid token");
        }

        public async Task<OperationResponse> UpdatePasswordAsync(string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identity.UserId);

            var usermanagerResponse =  await _unitOfWork.Users.UpdatePasswordAsync(user, currentPassword, newPassword);

            if (usermanagerResponse.IsSuccess)
                return OperationResponse.Succeeded("Password has been updated");

            return OperationResponse.Failed("Password update failed", usermanagerResponse.Errors);
        }

        public async Task<OperationResponse> SendEmailUpdateConfirmationAsync(string newEmailAddress)
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identity.UserId);

            if (_identity.Email.Equals(newEmailAddress))
                return OperationResponse.Failed("Choose a new email address");

            string token = await _unitOfWork.Users.GenerateEmailUpdateToken(user, newEmailAddress);

            string url = $"{_authOptions.AppUrl}/api/auth/VerifyEmailUpdate?username={_identity.Username}&email={newEmailAddress}&token={token}";

            _mailService.SendEmail(user.Email, "Confirm your new email address", $"<h1>We received a request to change your email address</h1>" +
                    $"<p>Your new email address after confirmation is {newEmailAddress}<p>" + 
                    $"<p>To confirm the new email address click <a href={url}>here</a> </p>");

            return OperationResponse
                .Succeeded("The email modification token was succesfully sent to the user's email address");
        }

        public async Task<OperationResponse> UpdateEmailAddressAsync(string username, string newEmail, string token)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(username);

            if (user == null)
                return OperationResponse.Failed("User not found");

            var usermanagerResponse =  await _unitOfWork.Users.UpdateEmailAddressAsync(user, token, newEmail);

            if (usermanagerResponse.IsSuccess)
                return OperationResponse.Succeeded("Email address has been updated");

            return OperationResponse.Failed("Email address update failed", usermanagerResponse.Errors);
        }

        public async Task<OperationResponse> DeleteAccountAsync(string username) 
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(username);

            if (user == null)
                return OperationResponse.Failed("User not found");

            var result = await _unitOfWork.Users.DeleteUserAsync(user);

            if (result.IsSuccess)
                return OperationResponse.Succeeded("Your account has been deleted");

            return OperationResponse.Failed("The account deletion failed", result.Errors);
        }

        public async Task<OperationResponse> SendAccountDeletionConfirmationAsync() 
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identity.UserId);

            if (user == null)
                return OperationResponse.Failed("User not found");

            string url = $"{_authOptions.AppUrl}/Api/Auth/Delete/Action?username={_identity.Username}";

            _mailService.SendEmail(user.Email, "Account deletion", $"<h1>We received a request to delete your account</h1>" +                   
                    $"<p>To confirm it please click <a href={url}>here</a> </p>");

            return OperationResponse
                .Succeeded("The account deletion confirmation was succesfully sent to the user's email address");
        }
    }
}
