using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class StoneMineBehaviour : BuildingBehaviour
    {
        public override int Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.StoneProduction.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.StoneProduction.ProductionAmount = (int)Math.Ceiling(city.StoneProduction.ProductionAmount / 1.2);
            city.StoneProduction.UpgradeCost = upgradeCost;
            return city.StoneProduction.Stage -= 1;
        }

        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.StoneProduction.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.StoneProduction.ProductionAmount = (int)Math.Ceiling(city.StoneProduction.ProductionAmount * 1.2);
            city.StoneProduction.UpgradeCost = upgradeCost;
            return city.StoneProduction.Stage += 1;
        }
    }
}
