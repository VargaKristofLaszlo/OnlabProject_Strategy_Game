using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class CityDetails
    {
        public string CityName { get; set; }
        public int BarrackStage { get; set; }
        public Resources BarrackUpgradeCost { get; set; }
        public int CityHallStage { get; set; }
        public Resources CityHallUpgradeCost { get; set; }
        public int CityWallStage { get; set; }
        public Resources CityWallUpgradeCost { get; set; }
        public int FarmStage { get; set; }
        public Resources FarmUpgradeCost { get; set; }
        public int SilverMineStage { get; set; }
        public Resources SilverMineUpgradeCost { get; set; }
        public int StoneMineStage { get; set; }
        public Resources StoneMineUpgradeCost { get; set; }
        public int LumberStage { get; set; }
        public Resources LumberUpgradeCost { get; set; }
        public int WarehouseStage { get; set; }
        public Resources WarehouseUpgradeCost { get; set; }


    }
}
