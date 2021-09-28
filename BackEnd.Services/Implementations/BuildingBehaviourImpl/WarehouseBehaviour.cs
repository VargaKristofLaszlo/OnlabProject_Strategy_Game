using BackEnd.Models.Models;
using Services.Exceptions;
using Services.Interfaces;
using System;

namespace Services.Implementations.BuildingBehaviourImpl
{
    public class WarehouseBehaviour : IBuildingBehaviour
    {
        public City Downgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost.BuildingStage != city.Warehouse.Stage)
                throw new InvalidBuildingStageModificationException();

            city.Warehouse.MaxSilverStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxSilverStorageCapacity / 1.5);
            city.Warehouse.MaxStoneStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxStoneStorageCapacity / 1.5);
            city.Warehouse.MaxWoodStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxWoodStorageCapacity / 1.5);
            city.Warehouse.UpgradeCost = upgradeCost;
            city.Warehouse.Stage -= 1;
            city.Warehouse.BuildingCostId = upgradeCost.Id;
            return city;
        }


        public City Upgrade(City city, BuildingUpgradeCost upgradeCost)
        {
            if (upgradeCost == null)
            {
                city.Warehouse.MaxSilverStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxSilverStorageCapacity * 1.5);
                city.Warehouse.MaxStoneStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxStoneStorageCapacity * 1.5);
                city.Warehouse.MaxWoodStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxWoodStorageCapacity * 1.5);
                city.Warehouse.UpgradeCost = upgradeCost;
                city.Warehouse.Stage += 1;
                city.Warehouse.BuildingCostId = null;
            }
            else if (upgradeCost.BuildingStage - 2 != city.Warehouse.Stage)
                throw new InvalidBuildingStageModificationException();
            else
            {
                city.Warehouse.MaxSilverStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxSilverStorageCapacity * 1.5);
                city.Warehouse.MaxStoneStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxStoneStorageCapacity * 1.5);
                city.Warehouse.MaxWoodStorageCapacity = (int)Math.Ceiling(city.Warehouse.MaxWoodStorageCapacity * 1.5);
                city.Warehouse.UpgradeCost = upgradeCost;
                city.Warehouse.Stage += 1;
                city.Warehouse.BuildingCostId = upgradeCost.Id;
            }

            return city;
        }
    }
}
