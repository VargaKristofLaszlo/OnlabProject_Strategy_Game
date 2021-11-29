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

        public async Task AddNewBuildingJob(string userId, DateTime startingTime, DateTime finishTime, string buildingName,
            int newStage, int cityIndex)
        {


            await _db.UpgradeQueueItems
                .AddAsync(new UpgradeQueueItem()
                {
                    CreationTime = startingTime,
                    FinishTime = finishTime,
                    UserId = userId,
                    BuildingName = buildingName,
                    CityIndex = cityIndex,
                    NewStage = newStage
                });
        }

        public async Task AddNewUnitProductionJob(string userId, DateTime startingTime, DateTime finishTime, string unitName,
            int amount, int cityIndex)
        {
            await _db.UnitProductionQueueItems
                .AddAsync(new UnitProductionQueueItem()
                {
                    CreationTime = startingTime,
                    FinishTime = finishTime,
                    UserId = userId,
                    UnitName = unitName,
                    CityIndex = cityIndex,
                    Amount = amount
                });
        }



        public async Task<UpgradeQueueItem> GetBuildingJobByFinishTime(DateTime FinishTime, int cityIndex, string userId)
        {
            var job = await _db.UpgradeQueueItems
                .Where(x => userId.Equals(userId) && x.CityIndex == cityIndex)
                .EqualsUpToSeconds(FinishTime)
                .FirstAsync();

            return job;
        }

        public async Task<UnitProductionQueueItem> GetUnitJobByFinishTime(DateTime FinishTime, int cityIndex, string userId)
        {
            var items = await _db.UnitProductionQueueItems.ToListAsync();
            var count = items.Count;

            var job = await _db.UnitProductionQueueItems
                .Where(x => userId.Equals(userId) && x.CityIndex == cityIndex)
                .EqualsUpToSeconds(FinishTime)
                .FirstAsync();

            return job;
        }

        public async Task<UpgradeQueueItem> GetBuildingJobByJobId(string jobId)
        {
            return await _db.UpgradeQueueItems.FirstOrDefaultAsync(x => x.JobId.Equals(jobId));
        }
        public async Task<UnitProductionQueueItem> GetRecruitmentJobByJobId(string jobId)
        {
            return await _db.UnitProductionQueueItems.FirstOrDefaultAsync(x => x.JobId.Equals(jobId));
        }

        public async Task<DateTime> GetBuildingFinishTime(string userId, int cityIndex)
        {
            var timeNow = DateTime.Now;
            var finishTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, timeNow.Second);

            var query = _db.UpgradeQueueItems
                .Where(x => x.UserId.Equals(userId) && x.CityIndex == cityIndex);

            if (await query.AnyAsync() == false)
                return finishTime;

            else
            {
                return await query.MaxAsync(x => x.FinishTime);
            }
        }


        public async Task<DateTime> GetUnitFinishTime(string userId)
        {
            var timeNow = DateTime.Now;
            var finishTime = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, timeNow.Hour, timeNow.Minute, timeNow.Second);

            var query = _db.UnitProductionQueueItems
                .Where(x => x.UserId.Equals(userId));

            if (await query.AnyAsync() == false)
                return finishTime;

            else
            {
                return await query.MaxAsync(x => x.FinishTime);
            }
        }

        public async Task<List<UpgradeQueueItem>> GetUserBuildingQueue(string userId, int cityIndex)
        {
            return await _db.UpgradeQueueItems
                .Where(x => x.UserId.Equals(userId) && x.CityIndex == cityIndex)
                .ToListAsync();
        }

        public async Task<List<UpgradeQueueItem>> GetUserBuildingQueue(string userId)
        {
            return await _db.UpgradeQueueItems
                .Where(x => x.UserId.Equals(userId))
                .ToListAsync();
        }

        public async Task<List<UnitProductionQueueItem>> GetUserUnitProductionQueue(string userId, int cityIndex)
        {
            return await _db.UnitProductionQueueItems
                .Where(x => x.UserId.Equals(userId) && x.CityIndex == cityIndex)
                .ToListAsync();
        }

        public async Task<List<UnitProductionQueueItem>> GetUserUnitProductionQueue(string userId)
        {
            return await _db.UnitProductionQueueItems
                .Where(x => x.UserId.Equals(userId))
                .ToListAsync();
        }


        public void RemoveBuildingJob(UpgradeQueueItem hangFireJob)
        {
            _db.UpgradeQueueItems
                .Remove(hangFireJob);
        }
        public void RemoveBuildingJob(UnitProductionQueueItem hangFireJob)
        {
            _db.UnitProductionQueueItems
                .Remove(hangFireJob);
        }


    }
    internal static class DateTimeExtensions
    {
        public static IQueryable<UnitProductionQueueItem> EqualsUpToSeconds(this IQueryable<UnitProductionQueueItem> jobs, DateTime finishTime)
        {
            return jobs
               .Where(x =>
                   x.FinishTime.Year == finishTime.Year &&
                   x.FinishTime.Month == finishTime.Month &&
                   x.FinishTime.Day == finishTime.Day &&
                   x.FinishTime.Hour == finishTime.Hour &&
                   x.FinishTime.Minute == finishTime.Minute &&
                   x.FinishTime.Second == finishTime.Second);
        }
        public static IQueryable<UpgradeQueueItem> EqualsUpToSeconds(this IQueryable<UpgradeQueueItem> jobs, DateTime finishTime)
        {
            return jobs
               .Where(x =>
                   x.FinishTime.Year == finishTime.Year &&
                   x.FinishTime.Month == finishTime.Month &&
                   x.FinishTime.Day == finishTime.Day &&
                   x.FinishTime.Hour == finishTime.Hour &&
                   x.FinishTime.Minute == finishTime.Minute &&
                   x.FinishTime.Second == finishTime.Second);
        }
    }
}
