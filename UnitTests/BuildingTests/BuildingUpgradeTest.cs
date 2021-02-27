using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Moq;
using Services.Exceptions;
using Services.Implementations;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System;

namespace UnitTests.BuildingTests
{

    public class BuildingUpgradeTest
    {
        private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
        private readonly Mock<IIdentityContext> _mockedIdentityContext;
        private readonly Mock<IMapper> _mockedMapper;
        private readonly GameService _gameService;

        public BuildingUpgradeTest()
        {
            _mockedUnitOfWork = new Mock<IUnitOfWork>();
            _mockedIdentityContext = new Mock<IIdentityContext>();
            _mockedMapper = new Mock<IMapper>();
            _gameService = new GameService(_mockedUnitOfWork.Object, _mockedIdentityContext.Object, _mockedMapper.Object);
        }


        [Fact]
        public void UpgradeFailsDueToInvalidBuildingName()
        {
            Func<Task> act = () => _gameService.UpgradeBuilding(0, "randomBuildingName", 1);
            act.Should().Throw<NotFoundException>();
        }


        [Fact]
        public void UpgradeFailsDueToReachingMaxLevel()
        {
            _mockedUnitOfWork.Setup(uow => uow.UpgradeCosts.FindMaxStage(It.IsAny<string>())).ReturnsAsync(0);

            Func<Task> act = () => _gameService.UpgradeBuilding(0, "Barrack", 1);
            act.Should().Throw<BadRequestException>().WithMessage("The building is not upgradeable");
        }


        [Fact]
        public void UpgradeFailsDueToNotEnoughResources()
        {
            ApplicationUser user = new ApplicationUser();

            var mockedCity = new Mock<City>();
            mockedCity.Object.Resources = new Resources { Wood = 0, Stone = 0, Silver = 0, Population = 0 };

            var mockedUpgradeCost = new Mock<BuildingUpgradeCost>();
            mockedUpgradeCost.Object.UpgradeCost = new Resources { Wood = 10, Stone = 10, Silver = 10, Population = 10 };

            user.Cities.Add(mockedCity.Object);

            _mockedUnitOfWork.Setup(uow => uow.UpgradeCosts.FindMaxStage(It.IsAny<string>())).ReturnsAsync(1);
            _mockedUnitOfWork.Setup(uow => uow.Users.GetUserWithCities(It.IsAny<string>())).ReturnsAsync(user);
            _mockedUnitOfWork.Setup(uow => uow.Cities.FindCityById(It.IsAny<string>())).ReturnsAsync(mockedCity.Object);
            _mockedUnitOfWork.Setup(uow => uow.UpgradeCosts.FindUpgradeCost(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(mockedUpgradeCost.Object);

            Func<Task> act = () => _gameService.UpgradeBuilding(0, "Barrack", 1);
            act.Should().Throw<BadRequestException>().WithMessage("The city does not have enough resources");
        }

    }
}
