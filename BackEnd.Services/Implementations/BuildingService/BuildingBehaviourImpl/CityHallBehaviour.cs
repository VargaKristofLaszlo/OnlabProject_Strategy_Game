using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.Implementations.BuildingService.BuildingBehaviourImpl
{
    public class CityHallBehaviour : BuildingBehaviour
    {
        public override City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.CityHall.Stage)
                throw new InvalidBuildingStageModificationException();

            city.CityHall.UpgradeTimeReductionPercent -= 5;
            city.CityHall.UpgradeCost = upgradeCost;
            city.CityHall.Stage -= 1;
            city.CityHall.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public override City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.CityHall.UpgradeTimeReductionPercent += 5;
                city.CityHall.UpgradeCost = upgradeCost;
                city.CityHall.Stage += 1;
                city.CityHall.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage != city.CityHall.Stage)
                throw new InvalidBuildingStageModificationException();
            else 
            {
                city.CityHall.UpgradeTimeReductionPercent += 5;
                city.CityHall.UpgradeCost = upgradeCost;
                city.CityHall.Stage += 1;
                city.CityHall.BuildingCostId = upgradeCost.Id;
            }            
            return city;
        }
    }
}
