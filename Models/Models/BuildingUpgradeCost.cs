using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class BuildingUpgradeCost : Record
    {
        [Required]
        public Resources UpgradeCost { get; set; }
        
        [Required]
        public TimeSpan UpgradeTime { get; set; }
      

        // Used to connect the upgrade cost to the building
        [Required]
        public string BuildingName { get; set; }      
        [Required]
        public int BuildingStage { get; set; }
    }
}