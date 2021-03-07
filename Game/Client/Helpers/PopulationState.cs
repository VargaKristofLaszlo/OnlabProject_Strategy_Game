using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class PopulationState
    {
        public int Population { get; private set; }

        public event Action OnChange;

        public void SetPopulation(int newValue)
        {
            Population = newValue;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
