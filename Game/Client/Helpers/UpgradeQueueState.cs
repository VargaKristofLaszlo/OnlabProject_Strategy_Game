using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Shared.IServices;
using Game.Shared.Models;

namespace Game.Client.Helpers
{
    public class UpgradeQueueState
    {
        private  BuildingQueue _buildingQueue = new BuildingQueue();       
        public event Action OnChange;
        private readonly CityIndexState _cityIndexState;
        private readonly CityDetailsState _cityDetailsState;
        private readonly IViewService _viewService;
        private readonly IGameService _gameService;
        public System.Timers.Timer Timer { get; private set; }

        public UpgradeQueueState(CityIndexState cityIndexState, CityDetailsState cityDetailsState, IViewService viewService, IGameService gameService)
        {
            _cityIndexState = cityIndexState;
            _cityDetailsState = cityDetailsState;
            Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += (sender, eventArgs) => OnTimerCallback();
            Timer.AutoReset = true;
            Timer.Start();
            _viewService = viewService;
            _gameService = gameService;
        }

        private async void OnTimerCallback()
        {
            if(_buildingQueue.Queue.Count == 0)
            {
                Timer.Stop();
                return;
            }

            var item = _buildingQueue.Queue.First();

            item.UpgradeTime -= TimeSpan.FromSeconds(1);

            if (item.UpgradeTime.TotalSeconds == 0)
            {
                var upgradeCosts = await _viewService.GetBuildingUpgradeCostsByName(item.BuildingName);
                upgradeCosts = upgradeCosts.OrderBy(o => o.UpgradeCost.Wood).ToList();

                _buildingQueue.Queue.Remove(item);
                await _cityDetailsState.BuildingUpgrade(
                item.BuildingName,
                upgradeCosts.ElementAt(item.NewStage - 1).UpgradeCost,
                item.NewStage,
                _cityIndexState.Index);
            }          

            NotifyStateChanged();
        }

        public async Task InitBuildingQueue(string userId) 
        {
            _buildingQueue = await _viewService.GetBuildingQueueById(userId);

            _buildingQueue.Queue.Sort((x, y) => DateTime.Compare(x.FinishTime, y.FinishTime));

            QueueData previousItem = null;

            foreach (var item in _buildingQueue.Queue)
            {
                TimeSpan tmp;

                if (previousItem != null)
                    tmp = item.FinishTime - previousItem.FinishTime;

                else
                    tmp = item.FinishTime - DateTime.Now;

                item.UpgradeTime = new TimeSpan(tmp.Hours, tmp.Minutes, tmp.Seconds);
                previousItem = item;
            }

            NotifyStateChanged();
        }


        public void AddToQueue(QueueData entry)
        {
            _buildingQueue.Queue.Add(entry);
            Timer.Start();
            NotifyStateChanged();
        }
        public List<QueueData> GetUpgradeQueueOfCity(int cityIndex) 
        {
            return _buildingQueue.Queue.Where(x => x.CityIndex == cityIndex).ToList();          
        }
            
        public bool QueueIsFull(int cityIndeex) 
        {
            return _buildingQueue.Queue.Where(x => x.CityIndex == cityIndeex).Count() >= 3;
        }

        public async Task RemoveFromQueue(string jobId) 
        {
            var job = _buildingQueue.Queue.FirstOrDefault(x => x.JobId.Equals(jobId));

            if (job == null)
                return;

            _buildingQueue.Queue.Remove(job);

            await _gameService.RemoveUpgradeFromQueue(jobId);

            NotifyStateChanged();
        }


        public int GetUpgradeStage(string buildingName) 
        {
            var jobs = GetUpgradeQueueOfCity(_cityIndexState.Index);

            var buildingJobs = jobs.Where(x => x.BuildingName.Equals(buildingName));

            return buildingJobs.Count();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }


}
