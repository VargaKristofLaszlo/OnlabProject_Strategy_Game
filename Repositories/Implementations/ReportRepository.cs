using BackEnd.Models.Models;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _db;

        public ReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task CreateReport(Report report)
        {
            await _db.Reports.AddAsync(report);
        }

        public async Task<(List<Report> Reports, int Count)> GetReports(int pageNumber, int pageSize, string usename)
        {
            return (await _db.Reports  
                .Where(x => x.Defender.Equals(usename) || x.Attacker.Equals(usename))
                .Skip((pageNumber -1 ) * pageSize)
                .Take(pageSize)              
                .ToListAsync(), _db.Reports.ToListAsync().Result.Count);
        }
    }
}
