using BackEnd.Models.Models;
using Services.Implementations.DefensePhaseBehaviourImpl;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Implementations.AttackPhaseBehaviourImpl
{
    public class ArcheryAttackPhaseBehaviour : IAttackPhaseBehaviour
    {
        public (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            Action(AttackingTroops attackingTroops, DefendingTroops defendingTroops, int wallStage)
        {
            //Attack and defense values for the archery phase
            ArcheryDefensePhase archeryDefensePhase = new ArcheryDefensePhase();
            int defenseValue = archeryDefensePhase.CalculateDefenseValue(defendingTroops.ArcheryPhaseDefendingUnits);
            int attackValue = attackingTroops.CalculateAttackValue(attackingTroops.ArcheryPhaseTroops);

            //Apply the effects of the wall
            double wallDefenseMultiplier = wallStage * 0.05 + 1;
            defenseValue = (int)Math.Ceiling(defenseValue * wallDefenseMultiplier);
            defenseValue += wallStage * 10;

            if (attackingTroops.ArcheryPhaseTroops.Count == 0 || defendingTroops.ArcheryPhaseDefendingUnits.Count == 0)
                return (attackingTroops.ArcheryPhaseTroops, defendingTroops.ArcheryPhaseDefendingUnits);

            return attackingTroops.Fight(attackingTroops.ArcheryPhaseTroops, defendingTroops.ArcheryPhaseDefendingUnits, attackValue, defenseValue);
        }
    }
}
