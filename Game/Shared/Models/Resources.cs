using System.ComponentModel.DataAnnotations;

namespace Game.Shared.Models
{
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
