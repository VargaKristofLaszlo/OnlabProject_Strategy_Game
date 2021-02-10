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
        public int Wood { get; set; }

        [Required]
        public int Stone { get; set; }

        [Required]
        public int Silver { get; set; }

        [Required]
        public int Population { get; set; }
    }
}
