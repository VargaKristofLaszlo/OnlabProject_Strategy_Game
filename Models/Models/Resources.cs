using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace BackEnd.Models.Models
{
    [Owned]
    public class Resources
    {
        [Required]
        public int Wood { get; set; }

        [Required]
        public int Stone { get; set; }

        [Required]
        public int Silver { get; set; }

        [Required]
        public int Population { get; set; }
    }
}