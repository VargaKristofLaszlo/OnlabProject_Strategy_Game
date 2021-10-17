using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Game.Test.Data;
using Game.Test.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Commands;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.Tests
{
    public class ConquerTests : IClassFixture<SeedDataFixture>
    {
        private AttackOtherCity.Handler _handler;
        private AttackOtherCity.Command _command;

        public SeedDataFixture Fixture { get; private set; }

        public ConquerTests(SeedDataFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task Should_reduce_loyalty()
        {
            // Arrange
            await Fixture.SeedUnitTypes();
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            await Fixture.AddAttackingForcesToCity(TestDataConstants.UserIdOne);
            Mock();

            // Act
            await _handler.Handle(_command, new System.Threading.CancellationToken());

            // Assert
            var city = await Fixture.DbContext.Cities
                .FirstAsync(x => x.UserId.Equals(TestDataConstants.UserIdTwo));

            city.Loyalty.Should().BeLessOrEqualTo(80).And.BeGreaterOrEqualTo(70);
        }


        [Fact]
        public async Task Should_conquer_city()
        {
            // Arrange
            await Fixture.SeedUnitTypes();
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            await Fixture.AddAttackingForcesToCity(TestDataConstants.UserIdOne);
            Mock();

            // Act
            await _handler.Handle(_command, new System.Threading.CancellationToken());
            await _handler.Handle(_command, new System.Threading.CancellationToken());
            await _handler.Handle(_command, new System.Threading.CancellationToken());
            await _handler.Handle(_command, new System.Threading.CancellationToken());
            await _handler.Handle(_command, new System.Threading.CancellationToken());

            // Assert
            var user = await Fixture.DbContext.Users
                .Include(user => user.Cities)
                .FirstAsync(x => x.Id.Equals(TestDataConstants.UserIdOne));

            user.Cities.Count.Should().Be(2);
        }

        private void Mock()
        {
            var mockedIdentityContext = MockHelpers.MockIdentityContext();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            mockedUnitOfWork.MockGetUserWithCities(Fixture);

            mockedUnitOfWork.Setup(x => x.Units.GetUnitsInCityByBarrackId(It.Is<string>(id => id.Equals(TestDataConstants.BarrackIdOne))))
                .ReturnsAsync(Fixture.DbContext.UnitsInCities
                .Include(u => u.Unit)
                .Where(u => u.BarrackId.Equals(TestDataConstants.BarrackIdOne)));

            mockedUnitOfWork.Setup(x => x.Units.GetUnitsInCityByBarrackId(It.Is<string>(id => id.Equals(TestDataConstants.BarrackIdTwo))))
               .ReturnsAsync(Fixture.DbContext.UnitsInCities
               .Include(u => u.Unit)
               .Where(u => u.BarrackId.Equals(TestDataConstants.BarrackIdTwo)));

            mockedUnitOfWork.Setup(x => x.Units.GetAllUnitsAsync()).ReturnsAsync(
                Fixture.DbContext.Units);

            var noble = Fixture.DbContext.Units.First(x => x.Name.Equals("Noble"));

            mockedUnitOfWork.Setup(x => x.Units.FindUnitByName(It.Is<string>(unit => unit.Equals("Noble"))))
                .ReturnsAsync(Fixture.DbContext.Units
                .First(x => x.Name.Equals("Noble")));

            mockedUnitOfWork.Setup(x => x.Units.GetProducibleUnitTypes(It.IsAny<int>())).ReturnsAsync(
                Fixture.DbContext.Units
                .Include(x => x.UnitCost));

            _command = new AttackOtherCity.Command(new Shared.Models.Request.AttackRequest()
            {
                AttackedCityIndex = 0,
                AttackedUserId = TestDataConstants.UserIdTwo,
                AttackerCityIndex = 0,
                AttackType = Shared.Models.AttackType.Conquer,
                AttackingForces = new Dictionary<string, int>()
                {
                    { "Light Cavalry",  100 },
                    { "Noble", 1}
                }
            });

            var mockedReportSender = new Mock<IReportSender>();

            mockedReportSender.Setup(x => x.CreateAttackReport(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<Unit, int>>(), It.IsAny<Dictionary<Unit, int>>(),
                It.IsAny<Dictionary<string, int>>(), It.IsAny<IEnumerable<UnitsInCity>>(), It.IsAny<int>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));


            var mockedUsermanager = MockHelpers.MockUserManager<ApplicationUser>();

            mockedUsermanager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(
                Fixture.DbContext.Users.First(x => x.UserName.Equals(TestDataConstants.UserNameOne)));

            _handler = new AttackOtherCity.Handler(mockedUnitOfWork.Object, mockedIdentityContext.Object, mockedReportSender.Object, mockedUsermanager.Object);
        }
    }
}

