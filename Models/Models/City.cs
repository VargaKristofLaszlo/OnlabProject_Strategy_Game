using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Models
{
    public class City : Record
    {
        [Required]
        [StringLength(255)]
        public string CityName { get; set; }

        [Required]      
        public Resources Resources { get; set; }

        //Foreign keys        


        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(SilverProduction))]
        public string SilverProductionId { get; set; }
        [Required]
        ResourceProduction SilverProduction { get; set; }

        [ForeignKey(nameof(StoneProduction))]
        public string StoneProductionId { get; set; }
        [Required]
        ResourceProduction StoneProduction { get; set; }

        [ForeignKey(nameof(WoodProduction))]
        public string WoodProductionId { get; set; }
        [Required]
        ResourceProduction WoodProduction { get; set; }

        [ForeignKey(nameof(Barrack))]
        public string BarrackId { get; set; }
        [Required]
        public Barrack Barrack { get; set; }

        [ForeignKey(nameof(Farm))]
        public string FarmId { get; set; }
        [Required]
        public Farm Farm { get; set; }

        [ForeignKey(nameof(CityWall))]
        public string CityWallId { get; set; }
        [Required]
        public CityWall CityWall { get; set; }

        [ForeignKey(nameof(CityHall))]
        public string CityHallId { get; set; }
        [Required]
        public CityHall CityHall { get; set; }

        [ForeignKey(nameof(Warehouse))]
        public string WarehouseId { get; set; }
        [Required]
        public Warehouse Warehouse { get; set; }
    }
}