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
    public class HangFireRepository : IHangFireRepository
    {
        private readonly ApplicationDbContext _db;

        public HangFireRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNewJob(string userId, DateTime startingTime, DateTime finishTime, string buildingName,
            int newStage, int cityIndex)
        {
            

            await _db.HangFireJobs
                .AddAsync(new HangFireJob() 
                {
                    CreationTime = startingTime,
                    FinishTime = finishTime,                    
                    UserId = userId,
                    BuildingName = buildingName,
                    CityIndex = cityIndex,
                    NewStage = newStage
                });
        }

        public async Task<DateTime> GetFinishTime(string userId)
        {
            var timeNow = DateTime.Now;
            var finishTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, timeNow.Second);

            var query =  _db.HangFireJobs
                .Where(x => x.UserId.Equals(userId));

            if (await query.AnyAsync() == false)
                return finishTime;

            else
            {
               return await query.MaxAsync(x => x.FinishTime);
            }
        }

        public async Task<HangFireJob> GetJobByFinishTime(DateTime FinishTime)
        {
            var job = await _db.HangFireJobs
                .EqualsUpToSeconds(FinishTime)
                .FirstAsync();

            return job;
        }

        public async Task<List<HangFireJob>> GetUserBuildingQueue(string userId)
        {
            return await _db.HangFireJobs
                .Where(x => x.UserId.Equals(userId))
                .ToListAsync();
        }

        public void RemoveJob(HangFireJob hangFireJob)
        {
            _db.HangFireJobs
                .Remove(hangFireJob);
        }
    }
    internal static class DateTimeExtensions
    {
        public static IQueryable<HangFireJob> EqualsUpToSeconds(this IQueryable<HangFireJob> jobs, DateTime finishTime)
        {
            return jobs
                .Where(x =>
                x.FinishTime.Year == finishTime.Year &&
                x.FinishTime.Month == finishTime.Month &&
                x.FinishTime.Day == finishTime.Day &&
                x.FinishTime.Hour == finishTime.Hour &&
                x.FinishTime.Minute == finishTime.Minute &&
                x.FinishTime.Second == finishTime.Second
                );
        }
    }
}
