using BackEnd.Models.Models;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IAttackPhaseBehaviour
    {
        (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            Action(AttackingTroops attackingTroops, DefendingTroops defendingTroops, int wallStage);
    }
}
