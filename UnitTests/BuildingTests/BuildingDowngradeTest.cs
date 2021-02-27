using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Moq;
using Services.Exceptions;
using Services.Implementations;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.BuildingTests
{
    public class BuildingDowngradeTest
    {
        private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
        private readonly Mock<IIdentityContext> _mockedIdentityContext;
        private readonly Mock<IMapper> _mockedMapper;
        private readonly GameService _gameService;

        public BuildingDowngradeTest()
        {
            _mockedUnitOfWork = new Mock<IUnitOfWork>();
            _mockedIdentityContext = new Mock<IIdentityContext>();
            _mockedMapper = new Mock<IMapper>();
            _gameService = new GameService(_mockedUnitOfWork.Object, _mockedIdentityContext.Object, _mockedMapper.Object);
        }


        [Fact]
        public void DowngradeFailsDueToTooLowBuildingStage()
        {
            int newStage = 1;
            int cityIndex = 0;
            string buildingName = "Barrack";

            Func<Task> act = () => _gameService.DowngradeBuilding(cityIndex, buildingName, newStage);

            act.Should().Throw<BadRequestException>().WithMessage("The building can not be downgraded any further");
        }
    }
}
