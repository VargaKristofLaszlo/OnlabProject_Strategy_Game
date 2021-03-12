using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class BarrackBehaviour : IBuildingBehaviour
    {

        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Barrack.Stage)
                throw new InvalidBuildingStageModificationException();

            city.Barrack.UpgradeCost = upgradeCost;
            city.Barrack.Stage -= 1;
            city.Barrack.BuildingCostId = upgradeCost.Id;
            return city;
        }



        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.Barrack.UpgradeCost = upgradeCost;
                city.Barrack.Stage += 1;
                city.Barrack.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage - 2 != city.Barrack.Stage)
                throw new InvalidBuildingStageModificationException();
            else
            {
                city.Barrack.UpgradeCost = upgradeCost;
                city.Barrack.Stage += 1;
                city.Barrack.BuildingCostId = upgradeCost.Id;
            }


            return city;
        }
    }
}
