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
        public System.Timers.Timer Timer { get; private set; }

        public UpgradeQueueState(CityIndexState cityIndexState, CityDetailsState cityDetailsState, IViewService viewService)
        {
            _cityIndexState = cityIndexState;
            _cityDetailsState = cityDetailsState;
            Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += (sender, eventArgs) => OnTimerCallback();
            Timer.AutoReset = true;
            Timer.Start();
            _viewService = viewService;
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

            foreach (var item in _buildingQueue.Queue)
            {
                var tmp = item.FinishTime - DateTime.Now;
                item.UpgradeTime = new TimeSpan(tmp.Hours, tmp.Minutes, tmp.Seconds);
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

        private void NotifyStateChanged() => OnChange?.Invoke();
    }


}
