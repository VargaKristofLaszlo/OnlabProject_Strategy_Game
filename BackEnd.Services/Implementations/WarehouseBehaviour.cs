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
    public class WarehouseBehaviour : BuildingBehaviour
    {
        public override int Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Warehouse.Stage - 1)
                throw new InvalidBuildingStageModificationException();

            city.Warehouse.MaxSilverStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxSilverStorageCapacity / 1.5);
            city.Warehouse.MaxStoneStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxStoneStorageCapacity / 1.5);
            city.Warehouse.MaxWoodStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxWoodStorageCapacity / 1.5);
            city.Warehouse.UpgradeCost = upgradeCost;
            return city.Warehouse.Stage -= 1;
        }


        public override int Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Warehouse.Stage + 1)
                throw new InvalidBuildingStageModificationException();

            city.Warehouse.MaxSilverStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxSilverStorageCapacity * 1.5);
            city.Warehouse.MaxStoneStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxStoneStorageCapacity * 1.5);
            city.Warehouse.MaxWoodStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxWoodStorageCapacity * 1.5);
            city.Warehouse.UpgradeCost = upgradeCost;
            return city.Warehouse.Stage += 1;
        }
    }
}
