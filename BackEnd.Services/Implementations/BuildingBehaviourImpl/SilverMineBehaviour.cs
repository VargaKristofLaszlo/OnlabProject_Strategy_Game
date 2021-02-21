using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class SilverMineBehaviour : BuildingBehaviour
    {
        public override int Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.SilverProduction.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.SilverProduction.ProductionAmount = (int)Math.Ceiling(city.SilverProduction.ProductionAmount / 1.2);
            city.SilverProduction.UpgradeCost = upgradeCost;
            return city.SilverProduction.Stage -= 1;
        }

        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.SilverProduction.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.SilverProduction.ProductionAmount = (int)Math.Ceiling(city.SilverProduction.ProductionAmount * 1.2);
            city.SilverProduction.UpgradeCost = upgradeCost;
            return city.SilverProduction.Stage += 1;
        }
    }
}
