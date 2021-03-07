using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.Implementations.BuildingService.BuildingBehaviourImpl
{
    public class FarmBehaviour : BuildingBehaviour
    {
        public override City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Farm.Stage)
                throw new InvalidBuildingStageModificationException();

            city.Farm.MaxPopulation -= 200;
            city.Farm.UpgradeCost = upgradeCost;
            city.Farm.Stage -= 1;
            city.Farm.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public override City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.Farm.MaxPopulation += 200;
                city.Farm.UpgradeCost = upgradeCost;
                city.Farm.Stage += 1;
                city.Farm.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage != city.Farm.Stage)
                throw new InvalidBuildingStageModificationException();
            else 
            {
                city.Farm.MaxPopulation += 200;
                city.Farm.UpgradeCost = upgradeCost;
                city.Farm.Stage += 1;
                city.Farm.BuildingCostId = upgradeCost.Id;
            }            
            return city;
        }
    }
}
