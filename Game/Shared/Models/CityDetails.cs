using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public record CityDetails
    {
        public DateTime LastResourceQueryTime{ get; init; }
        public string CityName { get; init; }
        public int BarrackStage { get; init; }
        public Resources BarrackUpgradeCost { get; init; }
        public int CityHallStage { get; init; }
        public Resources CityHallUpgradeCost { get; init; }
        public int CityWallStage { get; init; }
        public Resources CityWallUpgradeCost { get; init; }
        public int FarmStage { get; init; }
        public Resources FarmUpgradeCost { get; init; }
        public int SilverMineStage { get; init; }
        public Resources SilverMineUpgradeCost { get; init; }
        public int StoneMineStage { get; init; }
        public Resources StoneMineUpgradeCost { get; init; }
        public int LumberStage { get; init; }
        public Resources LumberUpgradeCost { get; init; }
        public int WarehouseStage { get; init; }
        public Resources WarehouseUpgradeCost { get; init; }
    }
}
