using Game.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Game.Shared.Models.Request
{
    public class UpgradeCostCreationRequest
    {
        [Required]
        public string BuildingName { get; set; }

        [Required]
        public int BuildingStage { get; set; }

        [Required]
        public Resources UpgradeCost { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The upgrade time  {0} must be greater than {1}.")]
        public int UpgradeTimeInSeconds { get; set; }
    }
}
