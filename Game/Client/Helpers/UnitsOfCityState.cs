using Game.Shared.Models;
using System;

namespace Game.Client.Helpers
{
    public class UnitsOfCityState
    {      
        public UnitsOfTheCity UnitsOfTheCity { get; private set; }

        public event Action OnChange;


        public void SetUnitsOfTheCity(UnitsOfTheCity value)
        {
            UnitsOfTheCity = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
