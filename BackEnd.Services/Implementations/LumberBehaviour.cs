using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations
{
    public class LumberBehaviour : BuildingBehaviour
    {
        public override int Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.WoodProduction.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.WoodProduction.ProductionAmount = (int)Math.Ceiling(city.WoodProduction.ProductionAmount / 1.2);
            city.WoodProduction.UpgradeCost = upgradeCost;
            return city.WoodProduction.Stage -= 1;
        }

        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.WoodProduction.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.WoodProduction.ProductionAmount = (int)Math.Ceiling(city.WoodProduction.ProductionAmount * 1.2);
            city.WoodProduction.UpgradeCost = upgradeCost;
            return city.WoodProduction.Stage += 1;
        }
    }
}
