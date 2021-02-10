using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEnd.Models.Models
{
    public class MaxBuildingStage : Record
    {
        [Required]
        public string BuildingName { get; set; }
        [Required]
        public int MaxStage { get; set; }
    }
}
