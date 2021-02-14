using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Models.Models
{
    public class Barrack : BuildingRecord
    {
        public List<UnitsInCity> UnitsInCity { get; set; } = new List<UnitsInCity>();

        public static Barrack Create(BuildingUpgradeCost upgradeCost)
        {
            return new Barrack 
            {
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                BuildingCostId = upgradeCost.Id,
                UpgradeCost = upgradeCost
            };
        }
    }
}