using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.Implementations.BuildingService.BuildingBehaviourImpl
{
    public class BarrackBehaviour : BuildingBehaviour
    {

        public override int Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Barrack.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.Barrack.UpgradeCost = upgradeCost;

            return city.Barrack.Stage -= 1;
        }



        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Barrack.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.Barrack.UpgradeCost = upgradeCost;

            return city.Barrack.Stage += 1;
        }
    }
}
