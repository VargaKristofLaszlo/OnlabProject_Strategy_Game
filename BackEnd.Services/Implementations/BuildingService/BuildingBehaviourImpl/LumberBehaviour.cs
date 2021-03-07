using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingService.BuildingBehaviourImpl
{
    public class LumberBehaviour : BuildingBehaviour
    {
        public override City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.WoodProduction.Stage)
                throw new InvalidBuildingStageModificationException();

            city.WoodProduction.ProductionAmount = (int)Math.Ceiling(city.WoodProduction.ProductionAmount / 1.2);
            city.WoodProduction.UpgradeCost = upgradeCost;
            city.WoodProduction.Stage -= 1;
            city.WoodProduction.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public override City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.WoodProduction.ProductionAmount = (int)Math.Ceiling(city.WoodProduction.ProductionAmount * 1.2);
                city.WoodProduction.UpgradeCost = upgradeCost;
                city.WoodProduction.Stage += 1;
                city.WoodProduction.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage != city.WoodProduction.Stage)
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
