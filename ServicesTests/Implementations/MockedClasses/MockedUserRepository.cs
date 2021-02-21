using BackEnd.Models.Models;
using BackEnd.Models.Response;
using BackEnd.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.Implementations.MockedClasses
{
    public class MockedUserRepository : IUsersRepository
    {
        Mock<ApplicationUser> attackingUser = new Mock<ApplicationUser>();
        Mock<ApplicationUser> defendingUser = new Mock<ApplicationUser>();
        Mock<City> attackingCity = new Mock<City>();
        Mock<City> defendingCity = new Mock<City>();
        private string attackingBarrackId;
        private string defendingBarrackId;

        public MockedUserRepository(string attackingUserId, string defendingUserId)
        {
            attackingUser.SetupGet(user => user.Id).Returns(attackingUserId);
            defendingUser.SetupGet(user => user.Id).Returns(defendingUserId);

            attackingUser.Setup(x => x.Cities.ElementAt(It.IsAny<int>())).Returns(attackingCity.Object);
            attackingCity.SetupGet(city => city.Barrack.Id).Returns(attackingBarrackId);

            defendingUser.Setup(x => x.Cities.ElementAt(It.IsAny<int>())).Returns(defendingCity.Object);            
        }

        public MockedUserRepository(string attackingUserId, string defendingUserId, string attackingBarrackId, string defendingBarrackId) : this(attackingUserId, defendingUserId)
        {
            this.attackingBarrackId = attackingBarrackId;
            this.defendingBarrackId = defendingBarrackId;
        }

        public async Task<ApplicationUser> GetUserWithCities(string userId)
        {
            if (attackingUser.Object.Id.Equals(userId))
                return attackingUser.Object;

            else if (defendingUser.Object.Id.Equals(userId))
                return defendingUser.Object;

            return null;
        }







        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UsermanagerResponse> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            throw new NotImplementedException();
        }

        public Task<UsermanagerResponse> ConfirmEmailUpdateAsync(ApplicationUser user, string token, string newEmail)
        {
            throw new NotImplementedException();
        }

        public Task<UsermanagerResponse> CreateUserAsync(ApplicationUser user, string password, string role)
        {
            throw new NotImplementedException();
        }

        public Task<UsermanagerResponse> DeleteUserAsync(ApplicationUser user, string token)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindUserByEmailOrNullAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindUserByIdOrNullAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindUserByUsernameOrNullAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<string> FindUserRoleAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateAccountDeletionTokenAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateEmailUpdateTokenAsync(ApplicationUser user, string newEmail)
        {
            throw new NotImplementedException();
        }

        public Task<string> GeneratePasswordResetTokenAsnyc(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<ApplicationUser> Users, int Count)> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<UsermanagerResponse> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<UsermanagerResponse> UpdateEmailAddressAsync(ApplicationUser user, string token, string newEmailAddress)
        {
            throw new NotImplementedException();
        }

        public Task<UsermanagerResponse> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
