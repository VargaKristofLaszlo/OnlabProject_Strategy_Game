using BackEnd.Models.Models;
using Services.Implementations.DefensePhaseBehaviourImpl;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services.Implementations.AttackPhaseBehaviourImpl
{
    public class InfantryAttackPhaseBehaviour : IAttackPhaseBehaviour
    {
        public (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            Action(AttackingTroops attackingTroops, DefendingTroops defendingTroops)
        {
            //Attack and defense values for the infantry phase
            InfantryDefensePhase infantryDefense = new InfantryDefensePhase();
            int defenseValue = infantryDefense.CalculateDefenseValue(defendingTroops.InfantryPhaseDefendingUnits);
            int attackValue = attackingTroops.CalculateAttackValue(attackingTroops.InfantryPhaseTroops);

            //Copying the prop values to fields so it can be passed as ref
            var attackingTroops_field = attackingTroops.InfantryPhaseTroops;
            var defendingTroops_field = defendingTroops.InfantryPhaseDefendingUnits;

            attackingTroops.Fight(ref attackingTroops_field, ref defendingTroops_field, attackValue, defenseValue);

            return (attackingTroops_field, defendingTroops_field);
        }
    }
}
