using BackEnd.Models.Models;
using Services.Implementations.AttackService.DefensePhaseBehaviourImpl;
using Services.Implementations.AttackService.Troops;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Implementations.AttackService.AttackPhaseBehaviourImpl
{
    public class CavalryAttackPhaseBehaviour : IAttackPhaseBehaviour
    {
        public (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            Action(AttackingTroops attackingTroops, DefendingTroops defendingTroops, int wallStage)
        {
            //Attack and defense values for the cavalry phase
            CavalryDefensePhase cavalryDefense = new CavalryDefensePhase();
            int defenseValue = cavalryDefense.CalculateDefenseValue(defendingTroops.CavalryPhaseDefendingUnits);
            int attackValue = attackingTroops.CalculateAttackValue(attackingTroops.CavalryPhaseTroops);

            //Apply the effects of the wall
            double wallDefenseMultiplier = wallStage * 0.05 + 1;
            defenseValue = (int)Math.Ceiling(defenseValue * wallDefenseMultiplier);
            defenseValue += wallStage * 10;

            if (attackingTroops.CavalryPhaseTroops.Count == 0 || defendingTroops.CavalryPhaseDefendingUnits.Count == 0)
                return (attackingTroops.CavalryPhaseTroops, defendingTroops.CavalryPhaseDefendingUnits);

            return attackingTroops.Fight(attackingTroops.CavalryPhaseTroops, defendingTroops.CavalryPhaseDefendingUnits, attackValue, defenseValue);
        }
    }
}
