using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class CityWall: BuildingRecord
    {
        [Required]
        public int DefensePoints { get; set; }

        public static CityWall Create(BuildingUpgradeCost upgradeCost) 
        {
            return new CityWall 
            {
                DefensePoints = 200,
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                BuildingCostId = upgradeCost.Id,
                UpgradeCost = upgradeCost
            };
        }
    }
}