using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class LumberBehaviour : IBuildingBehaviour
    {
        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.WoodProduction.Stage)
                throw new InvalidBuildingStageModificationException();

            city.WoodProduction.ProductionAmount = (int)Math.Ceiling(city.WoodProduction.ProductionAmount / 1.2);
            city.WoodProduction.UpgradeCost = upgradeCost;
            city.WoodProduction.Stage -= 1;
            city.WoodProduction.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.WoodProduction.ProductionAmount = (int)Math.Ceiling(city.WoodProduction.ProductionAmount * 1.2);
                city.WoodProduction.UpgradeCost = upgradeCost;
                city.WoodProduction.Stage += 1;
                city.WoodProduction.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage - 2 != city.WoodProduction.Stage)
                throw new InvalidBuildingStageModificationException();
            else
            {
                city.WoodProduction.ProductionAmount = (int)Math.Ceiling(city.WoodProduction.ProductionAmount * 1.2);
                city.WoodProduction.UpgradeCost = upgradeCost;
                city.WoodProduction.Stage += 1;
                city.WoodProduction.BuildingCostId = upgradeCost.Id;
            }

            return city;
        }
    }
}
