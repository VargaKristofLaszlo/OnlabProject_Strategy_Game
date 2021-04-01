using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class UpgradeQueueState
    {
        private List<QueueData> _queue  = new();       
        public event Action OnChange;
        private CityIndexState _cityIndexState;
        private CityDetailsState _cityDetailsState;
        public System.Timers.Timer _timer { get; private set; }

        public UpgradeQueueState(CityIndexState cityIndexState, CityDetailsState cityDetailsState)
        {
            _cityIndexState = cityIndexState;
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (sender, eventArgs) => OnTimerCallback();
            _timer.AutoReset = true;
            _timer.Start();
            _cityDetailsState = cityDetailsState;
        }

        private async void OnTimerCallback()
        {
            var item = GetFirstItemFromQueue(_cityIndexState.Index);

            if (item == null)
            {
                _timer.Stop();
                return;
            }

            item.UpgradeTime -= TimeSpan.FromSeconds(1);
            if (item.UpgradeTime.TotalSeconds == 0) 
            {
                RemoveFromQueue(_cityIndexState.Index, item.BuildingName, item.BuildingStage);
                await _cityDetailsState.BuildingUpgrade(
                item.BuildingName,
                item.UpgradeCost,
                item.BuildingStage,
                _cityIndexState.Index);
            }                
            NotifyStateChanged();
        }


        public void AddToQueue(QueueData entry)
        {
            _queue.Add(entry);
            _timer.Start();
            NotifyStateChanged();
        }
        public void RemoveFromQueue(int cityIndex, string buildingName, int buildingStage) 
        {
            var item = _queue.FirstOrDefault(x => x.CityIndex == cityIndex && 
            x.BuildingName.Equals(buildingName) &&
            x.BuildingStage == buildingStage);

            if(item != null)
                _queue.Remove(item);

            NotifyStateChanged();
        }
        public Queue<QueueData> GetUpgradeQueueOfCity(int cityIndex) 
        {            
            var cityQueueList = _queue.Where(x => x.CityIndex == cityIndex).ToList();
            return new Queue<QueueData>(cityQueueList);
        }

        public QueueData GetFirstItemFromQueue(int cityIndex) 
        {
            return _queue.FirstOrDefault(x => x.CityIndex == cityIndex);
        }

        public bool QueueIsFull(int cityIndeex) 
        {
            return _queue.Where(x => x.CityIndex == cityIndeex).Count() >= 3;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class QueueData 
    {
        public int CityIndex { get; set; }
        public string BuildingName { get; set; }
        public int BuildingStage { get; set; }
        public TimeSpan UpgradeTime { get; set; }
        public Game.Shared.Models.Resources UpgradeCost { get; set; }
    }
}
