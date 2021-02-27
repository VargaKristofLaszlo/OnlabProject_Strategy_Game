using BackEnd.Models.Models;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services.Implementations.AttackService.DefensePhaseBehaviourImpl
{
    public class CavalryDefensePhase : IDefensePhase
    {
        public int CalculateDefenseValue(Dictionary<Unit, int> troops)
        {
            int defenseValue = 0;

            foreach (var item in troops)
            {
                defenseValue += item.Key.CavalryDefensePoint * item.Value;
            }

            return defenseValue;
        }
    }
}
