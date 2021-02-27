using BackEnd.Models.Models;
using Services.Implementations.AttackService.Troops;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IAttackPhaseBehaviour
    {
        (Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops)
            Action(AttackingTroops attackingTroops, DefendingTroops defendingTroops, int wallStage);
    }
}
