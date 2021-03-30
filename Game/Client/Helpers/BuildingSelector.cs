using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public enum BuildingSelector
    {
        Barrack = 1,
        CityHall = 2,
        CityWall = 3,
        Farm = 4,
        Lumber = 5,
        SilverMine = 6,
        StoneMine = 7,
        Warehouse = 8
    }
    public class BuildingSelectorTransformer
    {
        public static string GetEnumName(string selector)
        {
            switch (selector)
            {
                case "Barrack": return "Barrack";
                case "CityHall": return "City hall";
                case "CityWall": return "City wall";
                case "Farm": return "Farm";
                case "Lumber": return "Lumber";
                case "SilverMine": return "Silver mine";
                case "StoneMine": return "Stone mine";
                case "Warehouse": return "Warehouse";
                default: return String.Empty;
            }
        }
    }
}
