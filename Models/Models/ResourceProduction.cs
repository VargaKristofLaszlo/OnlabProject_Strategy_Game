using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEnd.Models.Models
{
    public class ResourceProduction : BuildingRecord
    {
        [Required]
        public ResourceType ResourceType { get; set; }

        [Required]
        public int ProductionAmount { get; set; }

       
    }
}
