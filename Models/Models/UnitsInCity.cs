using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Models
{
    public class UnitsInCity : Record
    {
        [Required]
        public int Amount { get; set; }

        [ForeignKey(nameof(Barrack))]
        public string BarrackId { get; set; }
        public Barrack Barrack { get; set; }

        [ForeignKey(nameof(Unit))]
        public string UnitId { get; set; }
        [Required]
        public Unit Unit { get; set; } 
    }
}