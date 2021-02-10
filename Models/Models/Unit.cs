using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Models
{
    public class Unit : Record
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int AttackPoint { get; set; }

        [Required]
        public int DefensePoint { get; set; }

        [Required]
        public int MinBarrackStage { get; set; }

        [Required]
        public int CarryingCapacity { get; set; }

        [Required]
        public Resources UnitCost { get; set; }

        public List<UnitsInCity> UnitsInCity { get; set; } = new List<UnitsInCity>();
    }
}