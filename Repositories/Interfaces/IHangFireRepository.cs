using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IHangFireRepository
    {
        Task AddNewBuildingJob(string userId, DateTime startingTime, DateTime finishTime, string buildingName,
            int newStage, int cityIndex);
        Task AddNewUnitProductionJob(string userId, DateTime startingTime, DateTime finishTime, string unitName,
            int amount, int cityIndex);
        Task<DateTime> GetBuildingFinishTime(string userId, int cityIndex);
        Task<DateTime> GetUnitFinishTime(string userId);
        Task<UpgradeQueueItem> GetBuildingJobByFinishTime(DateTime FinishTime, int cityIndex, string userId);
        Task<UnitProductionQueueItem> GetUnitJobByFinishTime(DateTime finishTime, int cityIndex, string userId);
        Task<UpgradeQueueItem> GetBuildingJobByJobId(string jobId);
        Task<UnitProductionQueueItem> GetRecruitmentJobByJobId(string jobId);
        void RemoveBuildingJob(UpgradeQueueItem hangFireJob);
        void RemoveBuildingJob(UnitProductionQueueItem hangFireJob);
        Task<List<UpgradeQueueItem>> GetUserBuildingQueue(string userId, int cityIndex);
        Task<List<UpgradeQueueItem>> GetUserBuildingQueue(string userId);
        Task<List<UnitProductionQueueItem>> GetUserUnitProductionQueue(string userId, int cityIndex);
        Task<List<UnitProductionQueueItem>> GetUserUnitProductionQueue(string userId);
    }
}
