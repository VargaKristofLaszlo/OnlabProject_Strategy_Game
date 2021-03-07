using Game.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class CityResourceState
    {
        public int Wood { get; private set; }
        public int Stone { get; private set; }
        public int Silver { get; private set; }

        public event Action OnChange;

        public void SetResourceValue(Resources newValue)
        {            
            Wood = Wood - newValue.Wood;
            Stone = Stone - newValue.Stone;
            Silver = Silver - newValue.Silver;
            NotifyStateChanged();
        }

        public void SetInitValue(Resources newValue) 
        {
            Wood = newValue.Wood;
            Stone = newValue.Stone;
            Silver = newValue.Silver;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
