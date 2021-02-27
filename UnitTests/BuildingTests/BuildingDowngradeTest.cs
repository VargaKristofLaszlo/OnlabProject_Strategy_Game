using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using FluentAssertions;
using Moq;
using Services.Exceptions;
using Services.Implementations;
using Services.Implementations.BuildingService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.BuildingTests
{
    public class BuildingDowngradeTest
    {
        private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
        private readonly Mock<IIdentityContext> _mockedIdentityContext;       
        private readonly BuildingService _buildingService;

        public BuildingDowngradeTest()
        {
            _mockedUnitOfWork = new Mock<IUnitOfWork>();
            _mockedIdentityContext = new Mock<IIdentityContext>();           
            _buildingService = new BuildingService(_mockedUnitOfWork.Object, _mockedIdentityContext.Object);
        }


        [Fact]
        public void DowngradeFailsDueToTooLowBuildingStage()
        {
            int newStage = 1;
            int cityIndex = 0;
            string buildingName = "Barrack";

            Func<Task> act = () => _buildingService.DowngradeBuilding(cityIndex, buildingName, newStage);

            act.Should().Throw<BadRequestException>().WithMessage("The building can not be downgraded any further");
        }
    }
}
