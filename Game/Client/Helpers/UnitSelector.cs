using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public enum UnitSelector
    {
        Archer = 0,
        AxeFighter = 1,
        HeavyCavalry = 2,
        LightCavalry = 3,
        MountedArcher = 4,
        Spearman = 5,
        Swordsman = 6
    }
    public class UnitSelectorTransformer
    {      
        public static string GetEnumName(string selector)
        {
            switch (selector)
            {
                case "AxeFighter": return "Axe Fighter";
                case "HeavyCavalry": return "Heavy Cavalry";
                case "Swordsman": return "Swordsman";
                case "Spearman": return "Spearman";
                case "Archer": return "Archer";
                case "MountedArcher": return "Mounted Archer";
                case "LightCavalry": return "Light Cavalry";
                default: return String.Empty;
            }
        }
    }
}
