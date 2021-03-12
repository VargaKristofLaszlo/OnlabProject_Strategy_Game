using Game.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class CityResourceState
    {
        public int Population { get; private set; }
        public int Wood { get; private set; }
        public int Stone { get; private set; }
        public int Silver { get; private set; }
        public WarehouseCapacity WarehouseCapacity { get; private set; }

        public event Action OnChange;


        public void SetResourceValueAfterUpgrade(Resources newValue)
        {
            Population = Population - newValue.Population;
            Wood = Wood - newValue.Wood;
            Stone = Stone - newValue.Stone;
            Silver = Silver - newValue.Silver;
            NotifyStateChanged();
        }
        public void SetPopulation(int newValue)
        {
            Population = newValue;
            NotifyStateChanged();
        }

        public void SetInitValue(Resources newValue, WarehouseCapacity warehouseCapacity)
        {
            Population = newValue.Population;
            Wood = newValue.Wood;
            Stone = newValue.Stone;
            Silver = newValue.Silver;
            WarehouseCapacity = warehouseCapacity;
            NotifyStateChanged();
        }

        public void SetWarehouseCapacityAfterUpgrade(WarehouseCapacity warehouseCapacity) 
        {
            WarehouseCapacity = warehouseCapacity;
            NotifyStateChanged();
        }


        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
