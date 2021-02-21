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
    public class FarmBehaviour : BuildingBehaviour
    {
        public override int Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Farm.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.Farm.MaxPopulation -= 200;
            city.Farm.UpgradeCost = upgradeCost;
            return city.Farm.Stage -= 1;
        }

        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Farm.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.Farm.MaxPopulation += 200;
            city.Farm.UpgradeCost = upgradeCost;
            return city.Farm.Stage += 1;
        }
    }
}
