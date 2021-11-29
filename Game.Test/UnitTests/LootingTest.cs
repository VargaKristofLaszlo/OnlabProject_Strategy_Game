using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Game.Test.Data.Seed;
using Game.Test.UnitTests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Commands;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.Tests
{
    public class LootingTest : IClassFixture<SeedDataFixture>
    {
        private AttackOtherCity.Command _lootingCommand;
        private AttackOtherCity.Command _invalidUserCommand;
        private AttackOtherCity.Handler _handler;

        public SeedDataFixture Fixture { get; private set; }

        public LootingTest(SeedDataFixture fixture)
        {
            Fixture = fixture;
        }

        public LootingTest()
        {

        }

        [Fact]
        public async Task Should_be_successful_looting()
        {
            // Arrange
            await Fixture.SeedUnitTypes();
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            await Fixture.AddAttackingForcesToCity(TestDataConstants.UserIdOne);
            Mock();

            // Act
            Func<Task> act = async () => await _handler.Handle(_lootingCommand, new System.Threading.CancellationToken());

            // Assert
            await act.Should().NotThrowAsync();
            var city = Fixture.DbContext.Cities.First(x => x.UserId.Equals(TestDataConstants.UserIdOne));
            city.Resources.Wood.Should().BeGreaterThan(1000);
            city.Resources.Stone.Should().BeGreaterThan(1000);
            city.Resources.Silver.Should().BeGreaterThan(1000);
        }

        [Fact]
        public async Task Should_throw_exceptions()
        {
            // Arrange
            await Fixture.SeedUnitTypes();
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            await Fixture.AddAttackingForcesToCity(TestDataConstants.UserIdOne);
            Mock();

            // Act
            Func<Task> missingUserTest = async () => await _handler.Handle(_invalidUserCommand, new System.Threading.CancellationToken());


            // Assert
            await missingUserTest.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Attacker_should_win_with_losses()
        {
            // Arrange
            await Fixture.SeedUnitTypes();
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            await Fixture.AddAttackingForcesToCity(TestDataConstants.UserIdOne);
            Mock();

            var city = Fixture.DbContext.Cities
                .Include(x => x.Barrack)
                    .ThenInclude(b => b.UnitsInCity)
                .First(x => x.UserId.Equals(TestDataConstants.UserIdTwo));

            var spearman = Fixture.DbContext.Units.First(x => x.Name.Equals("Spearman"));

            city.Barrack.UnitsInCity.Add(new UnitsInCity() { Amount = 20, Unit = spearman, UnitId = spearman.Id, Barrack = city.Barrack, BarrackId = city.Barrack.Id });
            Fixture.DbContext.SaveChanges();

            // Act
            await _handler.Handle(_lootingCommand, new System.Threading.CancellationToken());

            // Assert

            var attackingCity = Fixture.DbContext.Cities
                .Include(x => x.Barrack)
                .ThenInclude(b => b.UnitsInCity)
                    .ThenInclude(u => u.Unit)
                .First(x => x.UserId.Equals(TestDataConstants.UserIdOne));

            attackingCity.Barrack.UnitsInCity.First(x => x.Unit.Name.Equals("Light Cavalry")).Amount.Should().BeLessThan(100);
        }

        [Fact]
        public async Task Attacker_should_loose()
        {
            // Arrange
            await Fixture.SeedUnitTypes();
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            await Fixture.AddAttackingForcesToCity(TestDataConstants.UserIdOne);
            Mock();

            var city = Fixture.DbContext.Cities
                .Include(x => x.Barrack)
                    .ThenInclude(b => b.UnitsInCity)
                .First(x => x.UserId.Equals(TestDataConstants.UserIdTwo));

            var spearman = Fixture.DbContext.Units.First(x => x.Name.Equals("Spearman"));

            city.Barrack.UnitsInCity.Add(new UnitsInCity() { Amount = 2000, Unit = spearman, UnitId = spearman.Id, Barrack = city.Barrack, BarrackId = city.Barrack.Id });

            Fixture.DbContext.SaveChanges();

            // Act
            await _handler.Handle(_lootingCommand, new System.Threading.CancellationToken());

            // Assert

            var attackingCity = Fixture.DbContext.Cities
                .Include(x => x.Barrack)
                .ThenInclude(b => b.UnitsInCity)
                    .ThenInclude(u => u.Unit)
                .First(x => x.UserId.Equals(TestDataConstants.UserIdOne));

            var defendingCity = Fixture.DbContext.Cities
                .Include(x => x.Barrack)
                .ThenInclude(b => b.UnitsInCity)
                    .ThenInclude(u => u.Unit)
                .First(x => x.UserId.Equals(TestDataConstants.UserIdTwo));

            attackingCity.Barrack.UnitsInCity.First(x => x.Unit.Name.Equals("Light Cavalry")).Amount.Should().Be(0);
            defendingCity.Barrack.UnitsInCity.First(x => x.Unit.Name.Equals("Spearman")).Amount.Should().BeLessThan(2000000);
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


            mockedUnitOfWork.Setup(x => x.Units.GetProducibleUnitTypes(It.IsAny<int>())).ReturnsAsync(
                Fixture.DbContext.Units
                .Include(x => x.UnitCost));

            _lootingCommand = new AttackOtherCity.Command(new Shared.Models.Request.AttackRequest()
            {
                AttackedCityIndex = 0,
                AttackedUserId = TestDataConstants.UserIdTwo,
                AttackerCityIndex = 0,
                AttackType = Shared.Models.AttackType.Looting,
                AttackingForces = new Dictionary<string, int>()
                {
                    { "Light Cavalry",  100 },
                    { "Noble", 1}
                }
            });

            _invalidUserCommand = new AttackOtherCity.Command(new Shared.Models.Request.AttackRequest()
            {
                AttackedCityIndex = 0,
                AttackedUserId = "Invalid user id",
                AttackerCityIndex = 0,
                AttackType = Shared.Models.AttackType.Looting,
                AttackingForces = new Dictionary<string, int>()
                {
                    { "Light Cavalry",  100 },
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
