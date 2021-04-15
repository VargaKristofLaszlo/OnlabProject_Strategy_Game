using BackEnd.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Report : Record
    {
        public DateTime CreationTime { get; set; }
        public int StolenWoodAmount { get; set; }
        public int StolenSilverAmount { get; set; }
        public int StolenStoneAmount { get; set; }
        public string Attacker { get; set; }
        public string AttackerCityName { get; set; }
        public string Defender { get; set; }
        public string DefendingCityName { get; set; }
        public int SwordsmanAttackerCountBefore { get; set; }
        public int HeavyCavalryAttackerCountBefore { get; set; }
        public int MountedArcherAttackerCountBefore { get; set; }
        public int LightCavalryAttackerCountBefore { get; set; }
        public int SpearmanAttackerCountBefore { get; set; }
        public int ArcherAttackerCountBefore { get; set; }
        public int AxeFighterAttackerCountBefore { get; set; }

        public int SwordsmanAttackerCountAfter { get; set; }
        public int HeavyCavalryAttackerCountAfter { get; set; }
        public int MountedArcherAttackerCountAfter { get; set; }
        public int LightCavalryAttackerCountAfter { get; set; }
        public int SpearmanAttackerCountAfter { get; set; }
        public int ArcherAttackerCountAfter { get; set; }
        public int AxeFighterAttackerCountAfter { get; set; }


        public int SwordsmanDefenderCountBefore { get; set; }
        public int HeavyCavalryDefenderCountBefore { get; set; }
        public int MountedArcherDefenderCountBefore { get; set; }
        public int LightCavalryDefenderCountBefore { get; set; }
        public int SpearmanDefenderCountBefore { get; set; }
        public int ArcherDefenderCountBefore { get; set; }
        public int AxeFighterDefenderCountBefore { get; set; }

        public int SwordsmanDefenderCountAfter { get; set; }
        public int HeavyCavalryDefenderCountAfter { get; set; }
        public int MountedArcherDefenderCountAfter { get; set; }
        public int LightCavalryDefenderCountAfter { get; set; }
        public int SpearmanDefenderCountAfter { get; set; }
        public int ArcherDefenderCountAfter { get; set; }
        public int AxeFighterDefenderCountAfter { get; set; }

    }
}
