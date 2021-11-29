using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Game.Test.Data.Seed;
using Game.Test.UnitTests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services.Commands;
using Services.Commands.Game;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.Tests
{
    public class SpyTests : IClassFixture<SeedDataFixture>
    {
        private RecruitNewSpy.Handler _recruitNewSpyHandler;
        private RecruitNewSpy.Command _recruitNewSpyCommand;
        private SpyOtherCity.Handler _spyOtherCityHandler;
        private SpyOtherCity.Command _spyOtherCityCommand;
        public SeedDataFixture Fixture { get; private set; }

        public SpyTests(SeedDataFixture fixture)
        {
            Fixture = fixture;
        }

        public SpyTests()
        {

        }

        [Fact]
        public async Task Should_recruit_a_spy()
        {
            // Arrange           
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock();

            // Act
            await _recruitNewSpyHandler.Handle(_recruitNewSpyCommand, new System.Threading.CancellationToken());

            // Assert
            var city = await Fixture.DbContext.Cities
                .Include(x => x.Tavern)
                .FirstAsync(x => x.UserId.Equals(TestDataConstants.UserIdOne));

            city.Tavern.SpyCount.Should().Be(1);
        }

        [Fact]
        public async Task Should_reach_maximum_number_of_spies()
        {
            // Arrange           
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            Mock();

            // Act
            await _recruitNewSpyHandler.Handle(_recruitNewSpyCommand, new System.Threading.CancellationToken());
            Func<Task> act = async () => await _recruitNewSpyHandler.Handle(_recruitNewSpyCommand, new System.Threading.CancellationToken());

            // Assert
            var city = await Fixture.DbContext.Cities
                .Include(x => x.Tavern)
                .FirstAsync(x => x.UserId.Equals(TestDataConstants.UserIdOne));

            city.Tavern.SpyCount.Should().Be(1);
            await act.Should().ThrowAsync<BadRequestException>().WithMessage("You cant recruit more spies, please upgrade your tavern");
        }

        [Fact]
        public async Task Should_be_successful_espionage()
        {
            // Arrange           
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            var tavern = await Fixture.DbContext.Taverns
                .Include(t => t.City)
                .FirstAsync(x => x.City.UserId.Equals(TestDataConstants.UserIdOne));

            tavern.SpyCount = 8;
            await Fixture.DbContext.SaveChangesAsync();
            Mock();

            // Act
            var report = await _spyOtherCityHandler.Handle(_spyOtherCityCommand, new System.Threading.CancellationToken());

            report.Should().NotBeNull();
            report.Successful.Should().BeTrue();
        }


        [Fact]
        public async Task Should_be_failed_espionage()
        {
            // Arrange           
            await Fixture.SeedUsersAsync();
            await Fixture.SeedBuildingUpgradeCostsAsync();
            await Fixture.AddCityToUsersAsync();
            var tavernOne = await Fixture.DbContext.Taverns
                .Include(t => t.City)
                .FirstAsync(x => x.City.UserId.Equals(TestDataConstants.UserIdOne));
            tavernOne.SpyCount = 8;

            var tavernTwo = await Fixture.DbContext.Taverns
                .Include(t => t.City)
                .FirstAsync(x => x.City.UserId.Equals(TestDataConstants.UserIdTwo));
            tavernTwo.SpyCount = 200;

            await Fixture.DbContext.SaveChangesAsync();
            Mock();

            // Act
            var report = await _spyOtherCityHandler.Handle(_spyOtherCityCommand, new System.Threading.CancellationToken());

            report.Should().NotBeNull();
            report.Successful.Should().BeFalse();
        }

        private void Mock()
        {
            var mockedIdentityContext = MockHelpers.MockIdentityContext();

            var mockedMapper = MockHelpers.MockMapper();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            mockedUnitOfWork.MockGetUserWithCities(Fixture);

            mockedUnitOfWork.MockFindCityById(Fixture);

            mockedUnitOfWork.Setup(x => x.UpgradeCosts.FindBuildingUpgradeCostsByName(It.Is<string>(name => name.Equals("Spy"))))
                .ReturnsAsync(Fixture.DbContext.BuildingUpgradeCosts.Where(x => x.BuildingName.Equals("Spy")).ToList());

            _recruitNewSpyHandler = new RecruitNewSpy.Handler(mockedUnitOfWork.Object, mockedIdentityContext.Object);
            _recruitNewSpyCommand = new RecruitNewSpy.Command(1, 0);

            _spyOtherCityHandler = new SpyOtherCity.Handler(mockedIdentityContext.Object, mockedUnitOfWork.Object, mockedMapper.Object);
            _spyOtherCityCommand = new SpyOtherCity.Command(new Shared.Models.Request.SpyRequest()
            {
                TargetCityIndex = 0,
                OwnerCityIndex = 0,
                UsedSpyCount = 8,
                UserId = TestDataConstants.UserIdTwo
            });

            mockedUnitOfWork.Setup(x => x.Units.GetUnitsInCityByBarrackId(It.IsAny<string>()))
                .ReturnsAsync(new List<UnitsInCity>());

            mockedUnitOfWork.Setup(x => x.Reports.CreateSpyReport(It.IsAny<SypReport>()))
                .Callback<SypReport>(report => Fixture.DbContext.SpyReports.Add(report));


            mockedUnitOfWork.Setup(x => x.CommitChangesAsync()).Invoking(_ => Fixture.DbContext.SaveChanges());
        }
    }
}
