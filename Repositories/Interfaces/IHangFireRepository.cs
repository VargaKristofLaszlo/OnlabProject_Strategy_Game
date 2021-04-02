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
        Task AddNewJob(string userId, DateTime startingTime, DateTime finishTime, string buildingName,
            int newStage, int cityIndex);        
        Task<DateTime> GetFinishTime(string userId);        
        Task<HangFireJob> GetJobByFinishTime(DateTime FinishTime);        
        void RemoveJob(HangFireJob hangFireJob);
        Task<List<HangFireJob>> GetUserBuildingQueue(string userId);
    }
}
