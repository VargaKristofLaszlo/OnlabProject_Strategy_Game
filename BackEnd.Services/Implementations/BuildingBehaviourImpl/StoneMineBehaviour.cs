using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class StoneMineBehaviour : IBuildingBehaviour
    {
        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.StoneProduction.Stage)
                throw new InvalidBuildingStageModificationException();

            city.StoneProduction.ProductionAmount = (int)Math.Ceiling(city.StoneProduction.ProductionAmount / 1.2);
            city.StoneProduction.UpgradeCost = upgradeCost;
            city.StoneProduction.Stage -= 1;
            city.StoneProduction.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.StoneProduction.ProductionAmount = (int)Math.Ceiling(city.StoneProduction.ProductionAmount * 1.2);
                city.StoneProduction.UpgradeCost = upgradeCost;
                city.StoneProduction.Stage += 1;
                city.StoneProduction.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage - 2 != city.StoneProduction.Stage)
                throw new InvalidBuildingStageModificationException();
            else
            {
                city.StoneProduction.ProductionAmount = (int)Math.Ceiling(city.StoneProduction.ProductionAmount * 1.2);
                city.StoneProduction.UpgradeCost = upgradeCost;
                city.StoneProduction.Stage += 1;
                city.StoneProduction.BuildingCostId = upgradeCost.Id;
            }
            return city;
        }
    }
}
