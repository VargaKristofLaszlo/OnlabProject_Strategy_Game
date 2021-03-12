using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class CityWallBehaviour : IBuildingBehaviour
    {
        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.CityWall.Stage)
                throw new InvalidBuildingStageModificationException();

            city.CityWall.DefensePoints -= 200;

            city.CityWall.UpgradeCost = upgradeCost;
            city.CityWall.Stage -= 1;
            city.CityWall.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.CityWall.DefensePoints += 200;
                city.CityWall.UpgradeCost = upgradeCost;
                city.CityWall.Stage += 1;
                city.CityWall.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage != city.CityWall.Stage)
                throw new InvalidBuildingStageModificationException();
            else
            {
                city.CityWall.DefensePoints += 200;
                city.CityWall.UpgradeCost = upgradeCost;
                city.CityWall.Stage += 1;
                city.CityWall.BuildingCostId = upgradeCost.Id;
            }
            return city;
        }
    }
}
