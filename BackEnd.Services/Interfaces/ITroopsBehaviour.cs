using BackEnd.Models.Models;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ITroopsBehaviour
    {
        void AddSurvivorsOfPreviousPhase(Dictionary<Unit, int> survivedTroops, Dictionary<Unit, int> troopsOfNextPhase);
    }
}
