using BackEnd.Models.Models;
using Services.Implementations.DefensePhaseBehaviourImpl;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Implementations.AttackPhaseBehaviourImpl
{
    public class InfantryAttackPhaseBehaviour : IAttackPhaseBehaviour
    {
        public (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            Action(AttackingTroops attackingTroops, DefendingTroops defendingTroops, int wallStage)
        {
            //Attack and defense values for the infantry phase
            InfantryDefensePhase infantryDefense = new InfantryDefensePhase();
            int defenseValue = infantryDefense.CalculateDefenseValue(defendingTroops.InfantryPhaseDefendingUnits);

            //Apply the effects of the wall
            double wallDefenseMultiplier = wallStage * 0.05 + 1;
            defenseValue = (int)Math.Ceiling(defenseValue * wallDefenseMultiplier);
            defenseValue += wallStage * 10;

            int attackValue = attackingTroops.CalculateAttackValue(attackingTroops.InfantryPhaseTroops);

            //Copying the prop values to fields so it can be passed as ref
            var attackingTroops_field = attackingTroops.InfantryPhaseTroops;
            var defendingTroops_field = defendingTroops.InfantryPhaseDefendingUnits;

            if (attackingTroops_field.Count == 0 || defendingTroops_field.Count == 0)
                return (attackingTroops_field, defendingTroops_field);

            attackingTroops.Fight(ref attackingTroops_field, ref defendingTroops_field, attackValue, defenseValue);

            return (attackingTroops_field, defendingTroops_field);
        }
    }
}
