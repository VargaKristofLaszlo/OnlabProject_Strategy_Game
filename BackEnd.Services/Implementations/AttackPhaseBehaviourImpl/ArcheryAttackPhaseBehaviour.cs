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

            //Copying the prop values to fields so it can be passed as ref
            var attackingTroops_field = attackingTroops.ArcheryPhaseTroops;
            var defendingTroops_field = defendingTroops.ArcheryPhaseDefendingUnits;

            if (attackingTroops.ArcheryPhaseTroops.Count == 0 || defendingTroops.ArcheryPhaseDefendingUnits.Count == 0)
                return (attackingTroops_field, defendingTroops_field);

            attackingTroops.Fight(ref attackingTroops_field, ref defendingTroops_field, attackValue, defenseValue);

            return (attackingTroops_field, defendingTroops_field);
        }
    }
}
