using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class CityIndexState
    {
        public int Index { get; private set; }
        public event Action OnChange;

        public void SetIndex(int newValue)
        {
            Index = newValue;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
