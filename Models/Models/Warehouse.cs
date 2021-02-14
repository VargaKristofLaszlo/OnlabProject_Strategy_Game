using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Models.Models
{
    public class Warehouse : BuildingRecord
    {
        public int MaxSilverStorageCapacity { get; set; }
        public int MaxStoneStorageCapacity { get; set; }
        public int MaxWoodStorageCapacity { get; set; }

        public static Warehouse Create(BuildingUpgradeCost upgradeCost) 
        {
            return new Warehouse
            {
                MaxSilverStorageCapacity = 1000,
                MaxStoneStorageCapacity = 1000,
                MaxWoodStorageCapacity = 1000,
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                UpgradeCost = upgradeCost,
                BuildingCostId = upgradeCost.Id
            };
        }
    }
}
