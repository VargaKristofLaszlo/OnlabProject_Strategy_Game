using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class Farm : BuildingRecord
    {
        [Required]
        public int MaxPopulation { get; set; }

        public static Farm Create(BuildingUpgradeCost upgradeCost) 
        {
            return new Farm
            {
                MaxPopulation = 100,                
                Stage = 0,
                BuildingName = upgradeCost.BuildingName,
                BuildingCostId = upgradeCost.Id,
                UpgradeCost = upgradeCost
            };
        }
    }
}