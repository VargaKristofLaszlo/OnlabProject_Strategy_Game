using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class CityHall : BuildingRecord
    {
        [Required]
        public int UpgradeTimeReductionPercent { get; set; }

        public static CityHall Create(BuildingUpgradeCost upgradeCost) 
        {
            return new CityHall
            {
                UpgradeTimeReductionPercent = 5,
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                BuildingCostId = upgradeCost.Id,
                UpgradeCost = upgradeCost
            };
        }
    }
}