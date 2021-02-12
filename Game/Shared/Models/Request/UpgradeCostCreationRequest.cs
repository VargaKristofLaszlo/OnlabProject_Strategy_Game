using Game.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Request
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
        public int UpgradeTimeInSeconds { get; set; }
    }
}
