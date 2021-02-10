using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEnd.Models.Models
{
    public abstract class BuildingRecord : Record
    {
        [Required]
        public int Stage { get; set; }

        [Required]
        public string BuildingName { get; set; }

        //Foreign keys

        [ForeignKey(nameof(UpgradeCost))]
        public string BuildingCostId { get; set; }        
        public BuildingUpgradeCost UpgradeCost { get; set; }

        
    }
}
