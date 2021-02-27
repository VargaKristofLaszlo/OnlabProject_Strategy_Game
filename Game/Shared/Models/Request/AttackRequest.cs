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
        public Dictionary<Unit, int> AttackingForces { get; set; }
        public string AttackedUserId { get; set; }
        public int AttackedCityIndex { get; set; }
        public int AttackerCityIndex { get; set; }
    }
}
