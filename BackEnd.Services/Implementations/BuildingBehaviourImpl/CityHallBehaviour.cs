using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class CityHallBehaviour : IBuildingBehaviour
    {
        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.CityHall.Stage)
                throw new InvalidBuildingStageModificationException();

            city.CityHall.UpgradeTimeReductionPercent -= 5;
            city.CityHall.UpgradeCost = upgradeCost;
            city.CityHall.Stage -= 1;
            city.CityHall.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
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
