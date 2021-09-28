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
    public class TavernBehaviour : IBuildingBehaviour
    {
        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Tavern.Stage)
                throw new InvalidBuildingStageModificationException();

            city.Tavern.MaximumSpyCount -= 2;

            city.Tavern.UpgradeCost = upgradeCost;
            city.Tavern.Stage -= 1;
            city.Tavern.BuildingCostId = upgradeCost.Id;
            return city;
        }

        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.Tavern.MaximumSpyCount += 2;
                city.Tavern.UpgradeCost = upgradeCost;
                city.Tavern.Stage += 1;
                city.Tavern.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage - 2 != city.Castle.Stage)
                throw new InvalidBuildingStageModificationException();
            else
            {
                city.Tavern.MaximumSpyCount += 2;
                city.Tavern.UpgradeCost = upgradeCost;
                city.Tavern.Stage += 1;
                city.Tavern.BuildingCostId = upgradeCost.Id;
            }
            return city;
        }
    }
}