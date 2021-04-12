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

        public void IncreaseUnitAmount(string unitName, int amount)
        {
            switch (unitName)
            {
                case "AxeFighter":
                    UnitsOfTheCity.AxeFighers += amount;
                    break;
                case "HeavyCavalry":
                    UnitsOfTheCity.HeavyCavalry += amount;
                    break;
                case "Swordsman":
                    UnitsOfTheCity.Swordsmans += amount;
                    break;
                case "Spearman":
                    UnitsOfTheCity.Spearmans += amount;
                    break;             
                case "Archer":
                    UnitsOfTheCity.Archers += amount;
                    break;
                case "MountedArcher":
                    UnitsOfTheCity.MountedArcher += amount;
                    break;
                case "LightCavalry":
                    UnitsOfTheCity.LightCavalry += amount;
                    break;                      
                default:
                    break;
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
