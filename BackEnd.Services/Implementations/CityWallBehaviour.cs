using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class CityWallBehaviour : BuildingBehaviour
    {
        public override int Downgrade(City city,BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.CityWall.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.CityWall.DefensePoints -= 200;

            city.CityWall.UpgradeCost = upgradeCost;
            return city.CityWall.Stage -= 1;
        }

        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.CityWall.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.CityWall.DefensePoints += 200;

            city.CityWall.UpgradeCost = upgradeCost;
            return city.CityWall.Stage += 1;
        }
    }
}
