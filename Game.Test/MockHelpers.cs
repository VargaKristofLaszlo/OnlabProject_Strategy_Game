using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using Game.Test.Data;
using Game.Test.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Game.Test
{
    public static class MockHelpers
    {
        public static StringBuilder LogMessage = new StringBuilder();

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        public static Mock<RoleManager<TRole>> MockRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new Mock<RoleManager<TRole>>(store, roles, MockLookupNormalizer(),
                new IdentityErrorDescriber(), null);
        }

        public static ILookupNormalizer MockLookupNormalizer()
        {
            var normalizerFunc = new Func<string, string>(i =>
            {
                if (i == null)
                    return null;
                else
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(i)).ToUpperInvariant();
            });
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            lookupNormalizer.Setup(i => i.NormalizeName(It.IsAny<string>())).Returns(normalizerFunc);
            lookupNormalizer.Setup(i => i.NormalizeEmail(It.IsAny<string>())).Returns(normalizerFunc);
            return lookupNormalizer.Object;
        }

        public static Mock<IIdentityContext> MockIdentityContext()
        {
            var mockedIdentityContext = new Mock<IIdentityContext>();

            mockedIdentityContext.Setup(x => x.UserId).Returns(TestDataConstants.UserIdOne);

            return mockedIdentityContext;
        }

        public static Mock<IUnitOfWork> MockGetUserWithCities(this Mock<IUnitOfWork> mockedUnitOfWork, SeedDataFixture fixture)
        {
            mockedUnitOfWork.Setup(x => x.Users.GetUserWithCities(It.Is<string>(id => id.Equals(TestDataConstants.UserIdOne)))).ReturnsAsync(
              fixture.DbContext.Users
              .Include(x => x.Cities)
              .First(x => x.Id.Equals(TestDataConstants.UserIdOne)));

            mockedUnitOfWork.Setup(x => x.Users.GetUserWithCities(It.Is<string>(id => id.Equals(TestDataConstants.UserIdTwo)))).ReturnsAsync(
                fixture.DbContext.Users
                .Include(x => x.Cities)
                .First(x => x.Id.Equals(TestDataConstants.UserIdTwo)));


            return mockedUnitOfWork;
        }

        public static Mock<IUnitOfWork> MockFindCityById(this Mock<IUnitOfWork> mockedUnitOfWork, SeedDataFixture fixture)
        {
            mockedUnitOfWork.Setup(x => x.Cities.FindCityById(It.Is<string>(id => id.Equals(TestDataConstants.UserIdOne)))).ReturnsAsync(
                fixture.DbContext.Users
                .Include(x => x.Cities)
                .First(x => x.Id.Equals(TestDataConstants.UserIdOne))
                .Cities[0]);

            mockedUnitOfWork.Setup(x => x.Cities.FindCityById(It.Is<string>(id => id.Equals(TestDataConstants.UserIdTwo)))).ReturnsAsync(
              fixture.DbContext.Users
              .Include(x => x.Cities)
              .First(x => x.Id.Equals(TestDataConstants.UserIdTwo))
              .Cities[0]);

            return mockedUnitOfWork;
        }

        public static Mock<IMapper> MockMapper()
        {
            var mockedMapper = new Mock<IMapper>();

            mockedMapper.Setup(x => x.Map<BackEnd.Models.Models.SypReport>(It.IsAny<SpyReport>()))
                .Returns(new BackEnd.Models.Models.SypReport());

            mockedMapper.Setup(x => x.Map<CityDetails>(It.IsAny<BackEnd.Models.Models.City>()))
                .Returns(new CityDetails());

            return mockedMapper;
        }
    }
}
