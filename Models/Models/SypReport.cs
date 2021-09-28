using BackEnd.Models.Models;
using Game.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Models.Models
{
    public class SypReport : Record
    {
        public DateTime CreationTime { get; set; }
        public string Attacker { get; set; }
        public ReportType ReportType { get; set; }
        public string CityName { get; set; }
        public int BarrackStage { get; set; }
        public int CityHallStage { get; set; }
        public int CityWallStage { get; set; }
        public int FarmStage { get; set; }
        public int SilverMineStage { get; set; }
        public int StoneMineStage { get; set; }
        public int LumberStage { get; set; }
        public int WarehouseStage { get; set; }
        public int CastleStage { get; set; }
        public int TavernStage { get; set; }

        public int Wood { get; set; }
        public int Stone { get; set; }
        public int Silver { get; set; }


        public int Spearmans { get; set; } = 0;
        public int Swordsmans { get; set; } = 0;
        public int AxeFighers { get; set; } = 0;
        public int Archers { get; set; } = 0;
        public int LightCavalry { get; set; } = 0;
        public int MountedArcher { get; set; } = 0;
        public int HeavyCavalry { get; set; } = 0;
        public int Noble { get; set; } = 0;

        public bool Successful { get; set; }
    }
}
