using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public record Report
    {
        public int StolenWoodAmount { get; init; }
        public int StolenSilverAmount { get; init; }
        public int StolenStoneAmount { get; init; }
        public DateTime CreationTime { get; init; }
        public string Attacker { get; init; }
        public string AttackerCityName { get; init; }
        public string Defender { get; init; }
        public string DefendingCityName { get; init; }
        public int SwordsmanAttackerCountBefore { get; init; }
        public int HeavyCavalryAttackerCountBefore { get; init; }
        public int MountedArcherAttackerCountBefore { get; init; }
        public int LightCavalryAttackerCountBefore { get; init; }
        public int SpearmanAttackerCountBefore { get; init; }
        public int ArcherAttackerCountBefore { get; init; }
        public int AxeFighterAttackerCountBefore { get; init; }

        public int SwordsmanAttackerCountAfter { get; init; }
        public int HeavyCavalryAttackerCountAfter { get; init; }
        public int MountedArcherAttackerCountAfter { get; init; }
        public int LightCavalryAttackerCountAfter { get; init; }
        public int SpearmanAttackerCountAfter { get; init; }
        public int ArcherAttackerCountAfter { get; init; }
        public int AxeFighterAttackerCountAfter { get; init; }


        public int SwordsmanDefenderCountBefore { get; init; }
        public int HeavyCavalryDefenderCountBefore { get; init; }
        public int MountedArcherDefenderCountBefore { get; init; }
        public int LightCavalryDefenderCountBefore { get; init; }
        public int SpearmanDefenderCountBefore { get; init; }
        public int ArcherDefenderCountBefore { get; init; }
        public int AxeFighterDefenderCountBefore { get; init; }

        public int SwordsmanDefenderCountAfter { get; init; }
        public int HeavyCavalryDefenderCountAfter { get; init; }
        public int MountedArcherDefenderCountAfter { get; init; }
        public int LightCavalryDefenderCountAfter { get; init; }
        public int SpearmanDefenderCountAfter { get; init; }
        public int ArcherDefenderCountAfter { get; init; }
        public int AxeFighterDefenderCountAfter { get; init; }
    }
}
