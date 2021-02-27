using BackEnd.Models.Models;
using FluentAssertions;
using Moq;
using Services.Exceptions;
using Services.Implementations.BuildingBehaviourImpl;
using System;
using Xunit;

namespace UnitTests.BuildingTests
{
    public class BuildingBehaviourTest
    {

        [Fact]
        public void UpgradeCompletedStageIncreased()
        {
            var mockedBarrack = new Mock<Barrack>();
            mockedBarrack.Object.Stage = 0;

            var mockedCity = new Mock<City>();
            mockedCity.Object.Barrack = mockedBarrack.Object;

            var mockedUpgradeCost = new Mock<BuildingUpgradeCost>();
            mockedUpgradeCost.Object.BuildingStage = 1;

            var upgradeResult = new BarrackBehaviour().Upgrade(mockedCity.Object, mockedUpgradeCost.Object);

            upgradeResult.Should().Be(1, "the upgrade raised it's level by 1");
        }

        [Fact]
        public void UpgradeFailsDueToMultipleStageIncreasesAtOnce()
        {
            var mockedBarrack = new Mock<Barrack>();
            mockedBarrack.Object.Stage = 0;

            var mockedCity = new Mock<City>();
            mockedCity.Object.Barrack = mockedBarrack.Object;

            var mockedUpgradeCost = new Mock<BuildingUpgradeCost>();
            mockedUpgradeCost.Object.BuildingStage = 3;

            var barrackBehaviour = new BarrackBehaviour();

            Action act = () => new BarrackBehaviour().Upgrade(mockedCity.Object, mockedUpgradeCost.Object);
            act.Should().Throw<InvalidBuildingStageModificationException>();
        }

        [Fact]
        public void DowngradeCompletedStageDecreased()
        {
            var mockedBarrack = new Mock<Barrack>();
            mockedBarrack.Object.Stage = 3;

            var mockedCity = new Mock<City>();
            mockedCity.Object.Barrack = mockedBarrack.Object;

            var mockedUpgradeCost = new Mock<BuildingUpgradeCost>();
            mockedUpgradeCost.Object.BuildingStage = 2;

            var downgradeResult = new BarrackBehaviour().Downgrade(mockedCity.Object, mockedUpgradeCost.Object);

            downgradeResult.Should().Be(2, "the downgrade lowered it's level by 1");
        }


        [Fact]
        public void DowngradeFailsDueToMultipleStageDecreasesAtOnce()
        {
            var mockedBarrack = new Mock<Barrack>();
            mockedBarrack.Object.Stage = 4;

            var mockedCity = new Mock<City>();
            mockedCity.Object.Barrack = mockedBarrack.Object;

            var mockedUpgradeCost = new Mock<BuildingUpgradeCost>();
            mockedUpgradeCost.Object.BuildingStage = 2;

            var barrackBehaviour = new BarrackBehaviour();

            Action act = () => new BarrackBehaviour().Downgrade(mockedCity.Object, mockedUpgradeCost.Object);
            act.Should().Throw<InvalidBuildingStageModificationException>();
        }

    }
}
