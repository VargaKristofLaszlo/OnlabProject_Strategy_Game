using BackEnd.Models.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DefendingTroops : ITroopsBehaviour
    {
        private Dictionary<Unit, int> _infantryPhaseDefendingUnits = new Dictionary<Unit, int>();
        private Dictionary<Unit, int> _cavalryPhaseDefendingUnits = new Dictionary<Unit, int>();
        private Dictionary<Unit, int> _archeryPhaseDefendingUnits = new Dictionary<Unit, int>();

        public Dictionary<Unit, int> InfantryPhaseDefendingUnits { get => _infantryPhaseDefendingUnits;  set => _infantryPhaseDefendingUnits = value; }
        public Dictionary<Unit, int> CavalryPhaseDefendingUnits { get => _cavalryPhaseDefendingUnits;  set => _cavalryPhaseDefendingUnits = value; }
        public Dictionary<Unit, int> ArcheryPhaseDefendingUnits { get => _archeryPhaseDefendingUnits;  set => _archeryPhaseDefendingUnits = value; }

        public DefendingTroops(IEnumerable<UnitsInCity> defendingTroops, double infantryProvisionPercentage,
            double cavalryProvisionPercentage, double archeryProvisionPercentage)
        {
            foreach (var item in defendingTroops)
            {
                InfantryPhaseDefendingUnits.Add(item.Unit, (int)Math.Floor(item.Amount * infantryProvisionPercentage));
                CavalryPhaseDefendingUnits.Add(item.Unit, (int)Math.Floor(item.Amount * cavalryProvisionPercentage));
                ArcheryPhaseDefendingUnits.Add(item.Unit, (int)Math.Floor(item.Amount * archeryProvisionPercentage));
            }
        }

        public void AddSurvivorsOfPreviousPhase(Dictionary<Unit, int> survivedTroops, Dictionary<Unit,int> troopsOfNextPhase) 
        {
            if (survivedTroops == null)
                return;

            foreach (var item in survivedTroops)
            {
                troopsOfNextPhase[item.Key] += item.Value;
            }
        }
   }
}
