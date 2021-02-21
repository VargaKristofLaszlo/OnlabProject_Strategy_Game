using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class CityHallBehaviour : BuildingBehaviour
    {
        public override int Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.CityHall.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.CityHall.UpgradeTimeReductionPercent -= 5;
            city.CityHall.UpgradeCost = upgradeCost;
            return city.CityHall.Stage -= 1;
        }

        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.CityHall.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.CityHall.UpgradeTimeReductionPercent += 5;
            city.CityHall.UpgradeCost = upgradeCost;
            return city.CityHall.Stage += 1;
        }
    }
}
