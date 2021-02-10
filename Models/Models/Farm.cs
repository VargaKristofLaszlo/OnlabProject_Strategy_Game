using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class Farm : BuildingRecord
    {
        [Required]
        public int MaxPopulation { get; set; }
    }
}