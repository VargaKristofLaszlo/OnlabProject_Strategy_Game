using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public class Unit
    {
        [Required]
        public int AttackPoint { get; set; }

        [Required]
        public int InfantryDefensePoint { get; set; }

        [Required]
        public int CavalryDefensePoint { get; set; }

        [Required]
        public int ArcherDefensePoint { get; set; }

        [Required]
        public int MinBarrackStage { get; set; }

        [Required]
        public int CarryingCapacity { get; set; }

        [Required]
        public Resources UnitCost { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
