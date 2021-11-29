using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Shared.IServices;
using Game.Shared.Models;

namespace Game.Client.Helpers
{
    public class UnitProductionQueueState
    {
        private UnitQueue _unitQueue = new UnitQueue();
        public event Action OnChange;
        private readonly CityIndexState _cityIndexState;
        private readonly CityDetailsState _cityDetailsState;
        private readonly IViewService _viewService;
        private readonly IGameService _gameService;
        private readonly UnitsOfCityState _unitsOfCityState;
        public System.Timers.Timer Timer { get; private set; }

        public UnitProductionQueueState(
            CityIndexState cityIndexState,
            CityDetailsState cityDetailsState,
            IViewService viewService,
            IGameService gameService,
            UnitsOfCityState unitsOfCityState)
        {
            _cityIndexState = cityIndexState;
            _cityDetailsState = cityDetailsState;
            Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += (sender, eventArgs) => OnTimerCallback();
            Timer.AutoReset = true;
            Timer.Start();
            _viewService = viewService;
            _gameService = gameService;
            _unitsOfCityState = unitsOfCityState;
        }

        private void OnTimerCallback()
        {
            if (_unitQueue.Queue.Count == 0)
            {
                Timer.Stop();
                return;
            }

            var item = _unitQueue.Queue.First();

            item.RecruitTime -= TimeSpan.FromSeconds(1);

            if (item.RecruitTime.TotalSeconds == 0)
            {
                _unitQueue.Queue.Remove(item);
                _unitsOfCityState.IncreaseUnitAmount(item.UnitName, item.Amount);
            }

            NotifyStateChanged();
        }

        public async Task InitRecruitmentQueue(string userId)
        {
            _unitQueue = await _viewService.GetUnitQueueById(userId);

            _unitQueue.Queue.Sort((x, y) => DateTime.Compare(x.FinishTime, y.FinishTime));

            UnitQueueData previousItem = null;

            foreach (var item in _unitQueue.Queue)
            {
                TimeSpan tmp;

                if (previousItem != null)
                    tmp = item.FinishTime - previousItem.FinishTime;

                else
                    tmp = item.FinishTime - DateTime.Now;

                item.RecruitTime = new TimeSpan(tmp.Hours, tmp.Minutes, tmp.Seconds);
                previousItem = item;
            }

            NotifyStateChanged();
        }


        public void AddToQueue(UnitQueueData entry)
        {
            _unitQueue.Queue.Add(entry);
            Timer.Start();
            NotifyStateChanged();
        }
        public List<UnitQueueData> GetUnitQueueOfCity(int cityIndex)
        {
            return _unitQueue.Queue.Where(x => x.CityIndex == cityIndex).ToList();
        }

        public bool QueueIsFull(int cityIndeex)
        {
            return _unitQueue.Queue.Where(x => x.CityIndex == cityIndeex).Count() >= 3;
        }

        public async Task RemoveFromQueue(string jobId)
        {
            var job = _unitQueue.Queue.FirstOrDefault(x => x.JobId.Equals(jobId));

            if (job == null)
                return;

            _unitQueue.Queue.Remove(job);

            await _gameService.RemoveRecruitmentFromQueue(jobId);

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }


}
