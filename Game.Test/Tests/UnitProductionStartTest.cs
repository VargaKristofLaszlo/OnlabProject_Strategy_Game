using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Game.Test.Data;
using Game.Test.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Commands.Game;
using Services.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.Tests
{
    public class UnitProductionStartTest : IClassFixture<SeedDataFixture>
    {
        private UnitProductionStart.Command _productionStartCommand;
        private UnitProductionStart.Command _productionStartCommandWithNotFoundException;
        private UnitProductionStart.Handler _productionStartHandler;

        public SeedDataFixture Fixture { get; private set; }

        public UnitProductionStartTest(SeedDataFixture fixture)
        {
            Fixture = fixture;
        }

        [Theory]
        [InlineData("Spearman", 1, 0)]
        [InlineData("Swordsman", 1, 0)]
        [InlineData("Axe Fighter", 1, 0)]
        [InlineData("Archer", 1, 0)]
        [InlineData("Light Cavalry", 1, 0)]
        [InlineData("Mounted Archer", 1, 0)]
        [InlineData("Heavy Cavalry", 1, 0)]
        public async Task Should_produce_unit(string name, int amount, int index)
        {
            // Arrange
            await Fixture.SeedUsersAsync();
            await Fixture.SeedUnitTypes();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock(name, amount, index);

            // Act
            var time = await _productionStartHandler.Handle(_productionStartCommand, new System.Threading.CancellationToken());
            System.Threading.Thread.Sleep(1000);

            // Assert
            (DateTime.Now - time).Should().BeLessThan(new TimeSpan(0, 0, 2));
        }

        [Fact]
        public async Task Should_throw_not_found_exception()
        {
            // Arrange
            await Fixture.SeedUsersAsync();
            await Fixture.SeedUnitTypes();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock();

            // Act
            Func<Task> act = async () => await _productionStartHandler.Handle(
                _productionStartCommandWithNotFoundException, new System.Threading.CancellationToken());

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Should_throw_bad_request_exception()
        {
            // Arrange
            await Fixture.SeedUsersAsync();
            await Fixture.SeedUnitTypes();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock("Swordsman", 2000, 0);

            // Act
            Func<Task> act = async () => await _productionStartHandler.Handle(_productionStartCommand, new System.Threading.CancellationToken());

            // Assert
            await act.Should().ThrowAsync<BadRequestException>().WithMessage("The city does not have enough resources");
        }


        private void Mock(string name = "", int amount = default, int index = default)
        {
            var mockedIdentityContext = MockHelpers.MockIdentityContext();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            mockedUnitOfWork.Setup(x => x.HangFire.GetUnitFinishTime(It.IsAny<string>()))
              .ReturnsAsync(DateTime.Now);

            mockedUnitOfWork.MockGetUserWithCities(Fixture);

            mockedUnitOfWork.MockFindCityById(Fixture);

            mockedUnitOfWork.Setup(x => x.Units.GetProducibleUnitTypes(It.IsAny<int>())).ReturnsAsync(
                Fixture.DbContext.Units
                .Include(x => x.UnitCost));

            mockedUnitOfWork.Setup(x => x.HangFire.AddNewUnitProductionJob(
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(Task.FromResult("Success"));

            _productionStartCommand = new UnitProductionStart.Command(
                new Shared.Models.Request.UnitProductionRequest() { Amount = amount, CityIndex = index, NameOfUnitType = name },
                mockedIdentityContext.Object);

            _productionStartCommandWithNotFoundException = new UnitProductionStart.Command(
                new Shared.Models.Request.UnitProductionRequest() { Amount = -1, CityIndex = 0, NameOfUnitType = "random name" },
                mockedIdentityContext.Object);

            _productionStartHandler = new UnitProductionStart.Handler(mockedUnitOfWork.Object);
        }
    }
}
