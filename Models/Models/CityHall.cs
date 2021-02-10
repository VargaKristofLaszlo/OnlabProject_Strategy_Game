using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class CityHall : BuildingRecord
    {
        [Required]
        public int UpgradeTimeReductionPercent { get; set; }
    }
}