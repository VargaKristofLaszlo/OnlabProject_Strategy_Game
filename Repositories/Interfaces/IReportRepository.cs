using BackEnd.Models.Models;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task CreateReport(Report report);
        Task CreateSpyReport(SypReport report);
        Task<(List<Report> Reports, int Count)> GetAttackReports(int pageNumber, int pageSize, string defenderName);
        Task<(List<SypReport> Reports, int Count)> GetSpyReports(int pageNumber, int pageSize, string attackerName);
    }
}
