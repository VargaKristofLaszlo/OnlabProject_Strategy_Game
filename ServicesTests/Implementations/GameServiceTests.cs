using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using Game.Shared.Models.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Profiles;
using Moq;
using Services.Implementations;
using Services.Interfaces;
using ServicesTests.Implementations.MockedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations.Tests
{
    [TestClass()]
    public class GameServiceTests
    {
       

        [TestMethod()]
        public void AttackOtherCityTest()
        {
            //Arrange
            IUnitOfWork mockedUnitOfWork = new MockedUnitOfWork("attackingUser", "defendingUser",
                "attackingBarrackId","defendingBarrackId");
            IMapper mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new UserCredentialProfile());
                config.AddProfile(new BuildingUpgradeCostProfile());
                config.AddProfile(new ResourceMapper());
                config.AddProfile(new UnitProfile());
                config.AddProfile(new CityProfile());
            }).CreateMapper();

            //Unit types and amounts used in the attack test
            var spearman = new Mock<Game.Shared.Models.Unit>();
            SpearmanGetSetup(spearman);
            int spearmanAmount = 10;

            Dictionary<Game.Shared.Models.Unit, int> attackingForces = new Dictionary<Game.Shared.Models.Unit, int>();
            attackingForces.Add(spearman.Object, spearmanAmount);

            AttackRequest attackRequest = new AttackRequest()
            {
                AttackType = AttackType.Looting,
                AttackingForces = new List<KeyValuePair<Game.Shared.Models.Unit, int>>()
                {
                    attackingForces.First()
                },
                AttackedUserId = "defendingUser",
                AttackedCityIndex = 0,
                AttackerCityIndex = 0
            };

            IdentityOptions identityOptions = new IdentityOptions()
            {
                UserId = "attackingUser"
            };


            IGameService gameService = new GameService(mockedUnitOfWork, identityOptions, mapper);

            MockedUnitRepository mockedUnitRepository = (MockedUnitRepository)mockedUnitOfWork.Units;

            // gameService.AttackOtherCity(attackRequest);


            Assert.AreEqual(10, mockedUnitRepository.attackingUnits.Object.Amount);

          //  Assert.AreNotEqual(mockedUnitRepository.attackingUnits.Object.Amount, 10);
          //  Assert.AreNotEqual(mockedUnitRepository.defendingUnits.Object.Amount, 10);

           
        }


        private void SpearmanGetSetup(Mock<Game.Shared.Models.Unit> spearman) 
        {
            spearman.SetupAllProperties();


           /* spearman.SetupGet(u => u.Name).Returns("Spearman");
            spearman.SetupGet(u => u.AttackPoint).Returns(10);
            spearman.SetupGet(u => u.UnitType).Returns(UnitType.Infantry);
            spearman.SetupGet(u => u.MinBarrackStage).Returns(1);
            spearman.SetupGet(u => u.CarryingCapacity).Returns(25);
            spearman.SetupGet(u => u.UnitCost.Wood).Returns(50);
            spearman.SetupGet(u => u.UnitCost.Stone).Returns(30);
            spearman.SetupGet(u => u.UnitCost.Silver).Returns(20);
            spearman.SetupGet(u => u.UnitCost.Population).Returns(1);
            spearman.SetupGet(u => u.ArcherDefensePoint).Returns(10);
            spearman.SetupGet(u => u.CavalryDefensePoint).Returns(45);
            spearman.SetupGet(u => u.InfantryDefensePoint).Returns(25);*/
        }
    }
}