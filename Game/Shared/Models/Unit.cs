using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Shared.Models
{
    public record Unit
    {
        [Required]
        public int AttackPoint { get; init; }

        [Required]
        public int InfantryDefensePoint { get; init; }

        [Required]
        public int CavalryDefensePoint { get; init; }

        [Required]
        public UnitType UnitType { get; init; }

        [Required]
        public int ArcherDefensePoint { get; init; }

        [Required]
        public int MinBarrackStage { get; init; }

        [Required]
        public int CarryingCapacity { get; init; }

        [Required]
        public Resources UnitCost { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
