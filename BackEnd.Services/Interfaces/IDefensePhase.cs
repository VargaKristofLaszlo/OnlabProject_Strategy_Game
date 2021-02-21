using BackEnd.Models.Models;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IDefensePhase
    {
        int CalculateDefenseValue(Dictionary<Unit, int> troops);
    }
}
