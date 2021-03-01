using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Interfaces;
using Services.Exceptions;
using Shared.Models.Request;
using Shared.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BackEnd.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthOptions _authOptions;
        private readonly IMailService _mailService;
        private readonly IIdentityContext _identityContext;

        public AuthenticationService(IUnitOfWork unitOfWork, AuthOptions authOptions,
            IMailService mailService, IIdentityContext identity)
        {
            _unitOfWork = unitOfWork;
            _authOptions = authOptions;
            _mailService = mailService;
            _identityContext = identity;
        }

     

        public async Task<LoginResponse> LoginAsync(UserLoginRequest request)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(request.Username);

            if (user == null || await _unitOfWork.Users.CheckPasswordAsync(user, request.Password) == false)
                throw new UnAuthorizedUserException("Invalid username or password");

            if (user.EmailConfirmed == false)
                throw new NotConfirmedAccountException("Please confirm your email address first");

            if (user.IsBanned)
                throw new BannedUserException("Your account has been banned");

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

            return new LoginResponse
            {
              //  AccessToken = tokenFactory.GenerateTokenString(jwtToken),
                ExpireDate = jwtToken.ValidTo
            };
        }



        public async Task ForgetPasswordAsync(string email)
        {
            var user = await _unitOfWork.Users.FindUserByEmailOrNullAsync(email);

            if (user == null)
                throw new NotFoundException();

            string token = await _unitOfWork.Users.GeneratePasswordResetTokenAsnyc(user);

            string url = $"{_authOptions.AppUrl}/ResetPassword?email={email}&token={token}";

            _mailService.SendEmail(email, "Reset password", $"<h1>Follow the instructions to reset your password</h1>" +
                    $"<p>To reset your password click <a href={url}>here</a> </p>");
        }

        public async Task ResetPasswordAsync(PasswordResetRequest resetRequest)
        {
            var user = await _unitOfWork.Users.FindUserByEmailOrNullAsync(resetRequest.Email);

            if (user == null)
                throw new NotFoundException();

            var usermanagerResponse = await _unitOfWork.Users.ResetPasswordAsync(user, resetRequest.Token, resetRequest.NewPassword);

            if (usermanagerResponse.IsSuccess == false)
                throw new OperationFailedException(usermanagerResponse.Errors.First());
        }

        public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identityContext.UserId);

            var usermanagerResponse = await _unitOfWork.Users.UpdatePasswordAsync(user, currentPassword, newPassword);

            if (usermanagerResponse.IsSuccess == false)
                throw new OperationFailedException(usermanagerResponse.Errors.First());
        }

        public async Task SendEmailUpdateConfirmationAsync(string newEmailAddress)
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identityContext.UserId);
            if (user == null)
                throw new NotFoundException();

            if (_identityContext.Email.Equals(newEmailAddress))
                throw new OperationFailedException("Choose a new email address");

            string token = await _unitOfWork.Users.GenerateEmailUpdateTokenAsync(user, newEmailAddress);

            string url = $"{_authOptions.AppUrl}/api/auth/VerifyEmailUpdate?username={_identityContext.Username}&email={newEmailAddress}&token={token}";

            _mailService.SendEmail(user.Email, "Confirm your new email address", $"<h1>We received a request to change your email address</h1>" +
                    $"<p>Your new email address after confirmation is {newEmailAddress}<p>" +
                    $"<p>To confirm the new email address click <a href={url}>here</a> </p>");
        }

        public async Task UpdateEmailAddressAsync(string username, string newEmail, string token)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(username);

            if (user == null)
                throw new NotFoundException();

            var usermanagerResponse = await _unitOfWork.Users.UpdateEmailAddressAsync(user, token, newEmail);

            if (usermanagerResponse.IsSuccess == false)
                throw new OperationFailedException(usermanagerResponse.Errors.First());
        }

        public async Task DeleteAccountAsync(string username, string token)
        {
            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(username);

            if (user == null)
                throw new NotFoundException();

            var result = await _unitOfWork.Users.DeleteUserAsync(user, token);

            if (result.IsSuccess == false)
                throw new BadRequestException("Account deletion failed");
        }

        public async Task SendAccountDeletionConfirmationAsync()
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identityContext.UserId);

            if (user == null)
                throw new NotFoundException();

            var accountDeletionToken = await _unitOfWork.Users.GenerateAccountDeletionTokenAsync(user);

            string url = $"{_authOptions.AppUrl}/Api/Auth/Delete/Action?username={_identityContext.Username}&token={accountDeletionToken}";

            _mailService.SendEmail(user.Email, "Account deletion", $"<h1>We received a request to delete your account</h1>" +
                    $"<p>To confirm it please click <a href={url}>here</a> </p>");
        }
    }
}
