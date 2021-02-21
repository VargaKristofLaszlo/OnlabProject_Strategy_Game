using BackEnd.Models.Models;
using Services.Implementations.DefensePhaseBehaviourImpl;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Implementations.AttackPhaseBehaviourImpl
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

            //Copying the prop values to fields so it can be passed as ref
            var attackingTroops_field = attackingTroops.CavalryPhaseTroops;
            var defendingTroops_field = defendingTroops.CavalryPhaseDefendingUnits;

            if (attackingTroops_field.Count == 0 || defendingTroops_field.Count == 0)
                return (attackingTroops_field, defendingTroops_field);

            attackingTroops.Fight(ref attackingTroops_field, ref defendingTroops_field, attackValue, defenseValue);

            return (attackingTroops_field, defendingTroops_field);
        }
    }
}
