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
        Task<(List<Report> Reports, int Count)> GetReports(int pageNumber, int pageSize, string defenderName);
    }
}
