using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Game.Test.UnitTests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Commands.Buildings;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.Tests
{
    public class BuildingUpgradeStartTest : IClassFixture<SeedDataFixture>
    {
        private UpgradeStart.Command _upgradeStartCommand;
        private UpgradeStart.Command _upgradeStartCommandWithNotFoundException;
        private UpgradeStart.Command _upgradeStartCommandNotUpgradeableBuilding;
        private UpgradeStart.Handler _upgradeStartHandler;

        public SeedDataFixture Fixture { get; private set; }

        public BuildingUpgradeStartTest(SeedDataFixture fixture)
        {
            Fixture = fixture;
        }

        public BuildingUpgradeStartTest()
        {

        }

        [Fact]
        public async Task Should_upgrade_building_stage()
        {
            // Arrange
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock();

            // Act
            var time = await _upgradeStartHandler.Handle(_upgradeStartCommand, new System.Threading.CancellationToken());
            System.Threading.Thread.Sleep(1000);

            // Assert
            (DateTime.Now - time).Should().BeLessThan(new TimeSpan(0, 0, 2));
        }

        [Fact]
        public async Task Should_throw_NotFoundException()
        {
            // Arrange
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock();

            // Act
            Func<Task> act = async () => await _upgradeStartHandler.Handle(_upgradeStartCommandWithNotFoundException, new System.Threading.CancellationToken());

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Should_throw_BadRequestException()
        {
            // Arrange
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock();

            // Act
            Func<Task> act = async () => await _upgradeStartHandler.Handle(_upgradeStartCommandNotUpgradeableBuilding, new System.Threading.CancellationToken());

            // Assert
            await act.Should().ThrowAsync<BadRequestException>().WithMessage("The building is not upgradeable");
        }

        private void Mock()
        {
            var mockedIdentityContext = MockHelpers.MockIdentityContext();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            mockedUnitOfWork.MockGetUserWithCities(Fixture);

            mockedUnitOfWork.MockFindCityById(Fixture);

            mockedUnitOfWork.Setup(x => x.UpgradeCosts.FindUpgradeCost(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(Fixture.DbContext.BuildingUpgradeCosts
                .Include(x => x.UpgradeCost)
                .First(x => x.BuildingName.Equals("Barrack")));

            mockedUnitOfWork.Setup(x => x.UpgradeCosts.FindMaxStage(It.IsAny<string>()))
                .ReturnsAsync(Fixture.DbContext.MaxBuildingStages
                .First(x => x.BuildingName.Equals("Barrack")).MaxStage);


            mockedUnitOfWork.Setup(x => x.HangFire.GetBuildingFinishTime(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(DateTime.Now);

            mockedUnitOfWork.Setup(x => x.HangFire.AddNewBuildingJob(
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(Task.FromResult("Success"));


            _upgradeStartCommand = new UpgradeStart.Command(0, "Barrack", 1, mockedIdentityContext.Object);
            _upgradeStartCommandWithNotFoundException = new UpgradeStart.Command(0, "invalid-buildingname", 1, mockedIdentityContext.Object);
            _upgradeStartCommandNotUpgradeableBuilding = new UpgradeStart.Command(0, "Barrack", 10, mockedIdentityContext.Object);
            _upgradeStartHandler = new UpgradeStart.Handler(mockedUnitOfWork.Object);
        }
    }
}
