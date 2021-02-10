using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class CityWall: BuildingRecord
    {
        [Required]
        public int DefensePoints { get; set; }
    }
}