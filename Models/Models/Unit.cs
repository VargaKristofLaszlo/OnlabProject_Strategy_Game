using Game.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Models
{
    public class Unit : Record
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int AttackPoint { get; set; }

        [Required]
        public int InfantryDefensePoint { get; set; }

        [Required]
        public int CavalryDefensePoint { get; set; }

        [Required]
        public int ArcherDefensePoint { get; set; }

        [Required]
        public UnitType UnitType { get; set; } 

        [Required]
        public int MinBarrackStage { get; set; }

        [Required]
        public int CarryingCapacity { get; set; }

        [Required]
        public Resources UnitCost { get; set; }

        [Required]
        public TimeSpan RecruitTime { get; set; }

        public List<UnitsInCity> UnitsInCity { get; set; } = new List<UnitsInCity>();
    }
}