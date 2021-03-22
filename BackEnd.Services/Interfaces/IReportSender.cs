using BackEnd.Models.Models;
using Game.Shared.Models.Response;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReportSender
    {
        Task CreateReport(string attacker, string attackerCityName, string defender, string defenderCityName,
            Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops,
             Dictionary<string, int> attackingUnits, IEnumerable<UnitsInCity> defendingUnits);

        Task<CollectionResponse<Game.Shared.Models.Report>> GetReports(int pageNumber, int pageSize, string defenderName);
    }
}
