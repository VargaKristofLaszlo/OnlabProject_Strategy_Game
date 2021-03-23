
using BackEnd.Models.Models;
using Game.Shared.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Implementations.AttackService.Troops
{
    public class AttackingTroops : ITroopsBehaviour
    {
        private int _infantryProvision = 0;
        private int _cavalryProvision = 0;
        private int _archeryProvision = 0;
        public Dictionary<BackEnd.Models.Models.Unit, int> InfantryPhaseTroops { get; set; } = new Dictionary<BackEnd.Models.Models.Unit, int>();
        public Dictionary<BackEnd.Models.Models.Unit, int> CavalryPhaseTroops { get; set; } = new Dictionary<BackEnd.Models.Models.Unit, int>();
        public Dictionary<BackEnd.Models.Models.Unit, int> ArcheryPhaseTroops { get; set; } = new Dictionary<BackEnd.Models.Models.Unit, int>();
        public double InfantryProvisionPercentage 
        { 
            get { return (double)_infantryProvision / (_infantryProvision + _cavalryProvision + _archeryProvision); }            
        } 
        public double CavalryProvisionPercentage
        {
            get { return (double)_cavalryProvision / (_infantryProvision + _cavalryProvision + _archeryProvision); }
        }
        public double ArcheryProvisionPercentage
        {
            get { return (double)_archeryProvision / (_infantryProvision + _cavalryProvision + _archeryProvision); }
        }


        public AttackingTroops(Dictionary<BackEnd.Models.Models.Unit, int> unitsAndAmounts)
        {
            foreach (var item in unitsAndAmounts)
            {
                switch (item.Key.UnitType)
                {
                    case UnitType.Infantry:
                        _infantryProvision += item.Key.UnitCost.Population * item.Value;
                        InfantryPhaseTroops.Add(item.Key, item.Value);
                        break;
                    case UnitType.Cavalry:
                        _cavalryProvision += item.Key.UnitCost.Population * item.Value;
                        CavalryPhaseTroops.Add(item.Key, item.Value);
                        break;
                    case UnitType.Archer:
                        _archeryProvision += item.Key.UnitCost.Population * item.Value;
                        ArcheryPhaseTroops.Add(item.Key, item.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        public int CalculateAttackValue(Dictionary<BackEnd.Models.Models.Unit, int> troops)
        {
            int res = 0;

            foreach (var item in troops)
            {
                res += item.Key.AttackPoint * item.Value;
            }

            return res;
        }

        public (Dictionary<BackEnd.Models.Models.Unit, int> attackingTroops, Dictionary<BackEnd.Models.Models.Unit, int> defendingTroops)
            Fight(Dictionary<BackEnd.Models.Models.Unit, int> attackingTroops, Dictionary<BackEnd.Models.Models.Unit, int> defendingTroops,
                int attackValue, int defenseValue)
        {
            double casualtyRatio;
            double DefenseDivAttack = (double)defenseValue / attackValue;
            double AttackDivDefense = (double)attackValue / defenseValue;

            if (attackValue > defenseValue)
            {
                casualtyRatio = Math.Sqrt(DefenseDivAttack) / AttackDivDefense;
                if (double.IsInfinity(casualtyRatio))
                    casualtyRatio = 0;

                // The attackers won the first phase all of the defending units are lost, the attackers have casualties
                foreach (var item in defendingTroops)
                {
                    defendingTroops[item.Key] = 0;
                }

                foreach (var item in attackingTroops)
                    attackingTroops[item.Key] = attackingTroops[item.Key] - (int)Math.Ceiling(attackingTroops[item.Key] * casualtyRatio);

                return (attackingTroops, defendingTroops);
            }

            casualtyRatio = Math.Sqrt(AttackDivDefense) / DefenseDivAttack;
            if (double.IsInfinity(casualtyRatio))
                casualtyRatio = 0;

            // The defenders won, they have casualties and the attackers lost everything            
            foreach (var item in attackingTroops)
            {
                attackingTroops[item.Key] = 0;
            }

            foreach (var item in defendingTroops)
                if (defendingTroops[item.Key] != 0) 
                {
                    defendingTroops[item.Key] = defendingTroops[item.Key] - (int)Math.Ceiling(defendingTroops[item.Key] * casualtyRatio);
                }               

            return (attackingTroops, defendingTroops);
        }

        public void AddSurvivorsOfPreviousPhase(Dictionary<BackEnd.Models.Models.Unit, int> survivedTroops,
            Dictionary<BackEnd.Models.Models.Unit, int> troopsOfNextPhase)
        {
            if (survivedTroops == null)
                return;

            if (troopsOfNextPhase.Count == 0)
                foreach (var item in survivedTroops)
                    troopsOfNextPhase.Add(item.Key, item.Value);


            else
                foreach (var item in survivedTroops)
                    troopsOfNextPhase.Add(item.Key, item.Value);
                        // troopsOfNextPhase[item.Key] += item.Value;

        }
    }
}
