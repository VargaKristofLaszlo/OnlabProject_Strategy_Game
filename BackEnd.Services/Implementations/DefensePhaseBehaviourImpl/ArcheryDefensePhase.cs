using BackEnd.Models.Models;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services.Implementations.DefensePhaseBehaviourImpl
{
    public class ArcheryDefensePhase : IDefensePhase
    {
        public int CalculateDefenseValue(Dictionary<Unit, int> troops)
        {
            int defenseValue = 0;

            foreach (var item in troops)
            {
                defenseValue += item.Key.ArcherDefensePoint * item.Value;
            }

            return defenseValue;
        }
    }
}
