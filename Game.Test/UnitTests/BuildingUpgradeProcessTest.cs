using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Game.Test.Data.Seed;
using Game.Test.UnitTests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Commands.Buildings;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.Tests
{
    public class BuildingUpgradeProcessTest : IClassFixture<SeedDataFixture>
    {
        private UpgradeProcess.Command _upgradeProcessCommand;
        private UpgradeProcess.Command _upgradeProcessCommandWithNotFoundException;
        private UpgradeProcess.Handler _upgradeProcessHandler;

        public SeedDataFixture Fixture { get; private set; }

        public BuildingUpgradeProcessTest(SeedDataFixture fixture)
        {
            Fixture = fixture;
        }

        public BuildingUpgradeProcessTest()
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
            await _upgradeProcessHandler.Handle(_upgradeProcessCommand, new System.Threading.CancellationToken());

            // Assert
            Fixture.DbContext.Cities.Include(city => city.Barrack).First(x => x.UserId.Equals(TestDataConstants.UserIdOne)).Barrack.Stage.Should().Be(1);
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
            Func<Task> act = async () => await _upgradeProcessHandler.Handle(_upgradeProcessCommandWithNotFoundException, new System.Threading.CancellationToken());

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
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
                .First(x => x.BuildingName.Equals("Barrack") && x.BuildingStage == 2));


            mockedUnitOfWork.Setup(x => x.HangFire.GetBuildingJobByFinishTime(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new Models.Models.UpgradeQueueItem());


            _upgradeProcessCommand = new UpgradeProcess.Command(0, "Barrack", 1, mockedIdentityContext.Object, DateTime.Now);
            _upgradeProcessCommandWithNotFoundException = new UpgradeProcess.Command(0, "invalid-buildingname", 1, mockedIdentityContext.Object, DateTime.Now);
            _upgradeProcessHandler = new UpgradeProcess.Handler(mockedUnitOfWork.Object);
        }
    }
}
