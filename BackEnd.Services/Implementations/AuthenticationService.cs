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
        private readonly IdentityOptions _identity;

        public AuthenticationService(IUnitOfWork unitOfWork, AuthOptions authOptions,
            IMailService mailService, IdentityOptions identity)
        {
            _unitOfWork = unitOfWork;
            _authOptions = authOptions;
            _mailService = mailService;
            _identity = identity;
        }

        public async Task RegisterUserAsync(UserCreationRequest request)
        {
            var user = new ApplicationUser()
            {
                UserName = request.Username,
                Email = request.Email
            };

            var userManagerResponse = await _unitOfWork.Users.CreateUserAsync(user, request.Password, "User");

            if (userManagerResponse.IsSuccess == false)
                throw new OperationFailedException(userManagerResponse.Errors.First());

            var token = await _unitOfWork.Users.GenerateEmailConfirmationTokenAsync(user);

            string url = $"{_authOptions.AppUrl}/api/Auth/ConfirmEmail?userid={user.Id}&token={token}";

            _mailService.SendEmail(user.Email, "Confirm your email", $"<h1>Welcome to my strategy game!</h1>" +
           $" <p>Please confirm your email by clicking <a href={url}>here</a></p>");
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
                AccessToken = tokenFactory.GenerateTokenString(jwtToken),
                ExpireDate = jwtToken.ValidTo
            };
        }


        public async Task ConfirmEmailAsync(string userId, string token)
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(userId);

            if (user == null)
                throw new NotFoundException();

            var usermanagerResponse = await _unitOfWork.Users.ConfirmEmailAsync(user, token);

            if (usermanagerResponse.IsSuccess == false)
                throw new OperationFailedException(usermanagerResponse.Errors.First());
           
            City city = await CreateCity(user);
            user.Cities.Add(city);
            await _unitOfWork.CommitChangesAsync();
        }

        private async Task<City> CreateCity(ApplicationUser user) 
        {
            //Get the upgrade costs which will be used to create the buildings
            BuildingUpgradeCost warehouseCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Warehouse", 1);
            BuildingUpgradeCost silverMineCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("SilverMine", 1);
            BuildingUpgradeCost stoneMineCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("StoneMine", 1);
            BuildingUpgradeCost lumberCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Lumber", 1);
            BuildingUpgradeCost farmCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Farm", 1);
            BuildingUpgradeCost cityWallCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("CityWall", 1);
            BuildingUpgradeCost cityhallCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("CityHall", 1);
            BuildingUpgradeCost barrackCost = await _unitOfWork.UpgradeCosts.FindUpgradeCost("Barrack", 1);

            //Create the buildings
            Warehouse warehouse = Warehouse.Create(warehouseCost);
            ResourceProduction silverMine = ResourceProduction.CreateResourceProductionBuilding(silverMineCost);
            ResourceProduction stoneMine = ResourceProduction.CreateResourceProductionBuilding(stoneMineCost);
            ResourceProduction lumber = ResourceProduction.CreateResourceProductionBuilding(lumberCost);
            Farm farm = Farm.Create(farmCost);
            CityWall cityWall = CityWall.Create(cityWallCost);
            CityHall cityHall = CityHall.Create(cityhallCost);
            Barrack barrack = Barrack.Create(barrackCost);

            //Add the buildings to the city
            return new City
            {
                CityName = $"{user.UserName}'s city",
                Resources = new Resources
                {
                    Wood = 1000,
                    Stone = 1000,
                    Silver = 1000,
                    Population = 100
                },
                UserId = user.Id,
                User = user,
                SilverProductionId = silverMine.Id,
                SilverProduction = silverMine,
                StoneProductionId = stoneMine.Id,
                StoneProduction = stoneMine,
                WoodProductionId = lumber.Id,
                WoodProduction = lumber,
                BarrackId = barrack.Id,
                Barrack = barrack,
                FarmId = farm.Id,
                Farm = farm,
                CityWallId = cityWall.Id,
                CityWall = cityWall,
                CityHallId = cityHall.Id,
                CityHall = cityHall,
                WarehouseId = warehouse.Id,
                Warehouse = warehouse
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
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identity.UserId);

            var usermanagerResponse = await _unitOfWork.Users.UpdatePasswordAsync(user, currentPassword, newPassword);

            if (usermanagerResponse.IsSuccess == false)
                throw new OperationFailedException(usermanagerResponse.Errors.First());
        }

        public async Task SendEmailUpdateConfirmationAsync(string newEmailAddress)
        {
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identity.UserId);
            if (user == null)
                throw new NotFoundException();

            if (_identity.Email.Equals(newEmailAddress))
                throw new OperationFailedException("Choose a new email address");

            string token = await _unitOfWork.Users.GenerateEmailUpdateTokenAsync(user, newEmailAddress);

            string url = $"{_authOptions.AppUrl}/api/auth/VerifyEmailUpdate?username={_identity.Username}&email={newEmailAddress}&token={token}";

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
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(_identity.UserId);

            if (user == null)
                throw new NotFoundException();

            var accountDeletionToken = await _unitOfWork.Users.GenerateAccountDeletionTokenAsync(user);

            string url = $"{_authOptions.AppUrl}/Api/Auth/Delete/Action?username={_identity.Username}&token={accountDeletionToken}";

            _mailService.SendEmail(user.Email, "Account deletion", $"<h1>We received a request to delete your account</h1>" +
                    $"<p>To confirm it please click <a href={url}>here</a> </p>");
        }
    }
}
