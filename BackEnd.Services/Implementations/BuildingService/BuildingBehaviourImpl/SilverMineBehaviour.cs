using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingService.BuildingBehaviourImpl
{
    public class SilverMineBehaviour : BuildingBehaviour
    {
        public override City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.SilverProduction.Stage)
                throw new InvalidBuildingStageModificationException();

            city.SilverProduction.ProductionAmount = (int)Math.Ceiling(city.SilverProduction.ProductionAmount / 1.2);
            city.SilverProduction.UpgradeCost = upgradeCost;
            city.SilverProduction.Stage -= 1;
            city.SilverProduction.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public override City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.SilverProduction.ProductionAmount = (int)Math.Ceiling(city.SilverProduction.ProductionAmount * 1.2);
                city.SilverProduction.UpgradeCost = upgradeCost;
                city.SilverProduction.Stage += 1;
                city.SilverProduction.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage != city.SilverProduction.Stage)
                throw new InvalidBuildingStageModificationException();
            else 
            {
                city.SilverProduction.ProductionAmount = (int)Math.Ceiling(city.SilverProduction.ProductionAmount * 1.2);
                city.SilverProduction.UpgradeCost = upgradeCost;
                city.SilverProduction.Stage += 1;
                city.SilverProduction.BuildingCostId = upgradeCost.Id;
            }           
            return city;
        }
    }
}
