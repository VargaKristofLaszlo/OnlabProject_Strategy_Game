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

        public async Task CreateSpyReport(SypReport report)
        {
            await _db.SpyReports.AddAsync(report);
        }

        public async Task<(List<Report> Reports, int Count)> GetAttackReports(int pageNumber, int pageSize, string usename)
        {
            return (await _db.Reports
                .Where(x => x.Defender.Equals(usename) || x.Attacker.Equals(usename))
                .OrderByDescending(x => x.CreationTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(), _db.Reports.ToListAsync().Result.Count);
        }

        public async Task<(List<SypReport> Reports, int Count)> GetSpyReports(int pageNumber, int pageSize, string attackerName)
        {
            return (await _db.SpyReports
                .Where(x => x.Attacker.Equals(attackerName))
                .OrderByDescending(x => x.CreationTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(), _db.SpyReports.ToListAsync().Result.Count);
        }
    }
}
