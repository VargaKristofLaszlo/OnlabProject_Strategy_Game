using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class CastleBehaviour : IBuildingBehaviour
    {
        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Castle.Stage)
                throw new InvalidBuildingStageModificationException();

            city.Castle.MaximumCoinCount = (city.Castle.Stage - 2) * 2 + 1;

            city.Castle.UpgradeCost = upgradeCost;
            city.Castle.Stage -= 1;
            city.Castle.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.Castle.MaximumCoinCount = 1;
                city.Castle.UpgradeCost = upgradeCost;
                city.Castle.Stage += 1;
                city.Castle.BuildingCostId = null;
                city.Castle.AvailableCoinCount = CalculateAvailableCoinCount();
            }
            else if (upgradeCost.BuildingStage != city.Castle.Stage)
                throw new InvalidBuildingStageModificationException();
            else
            {
                city.Castle.MaximumCoinCount = (city.Castle.Stage) * 2 + 1;
                city.Castle.UpgradeCost = upgradeCost;
                city.Castle.Stage += 1;
                city.Castle.BuildingCostId = upgradeCost.Id;
                city.Castle.AvailableCoinCount = CalculateAvailableCoinCount(city.Castle.Stage);
            }
            return city;
        }

        private int CalculateAvailableCoinCount(int stage = 1)
        {
            return (stage - 1) * 2 + 1;
        }
    }
}
