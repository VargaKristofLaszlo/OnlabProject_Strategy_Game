using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models.Request
{
    public class AttackRequest
    {
        public AttackType AttackType { get; set; }
        public int SpearmanAmount { get; set; }
        public int SwordsmanAmount { get; set; }
        public int AxeFighterAmount { get; set; }
        public int ArcherAmount { get; set; }
        public int LightCavalryAmount { get; set; }
        public int MountedArcherAmount { get; set; }
        public int HeavyCavalryAmount { get; set; }
        public string AttackedUsername { get; set; }
        public int AttackedCityIndex { get; set; }
        public int AttackerCityIndex { get; set; }
    }
}
