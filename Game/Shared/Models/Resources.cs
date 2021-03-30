using System.ComponentModel.DataAnnotations;

namespace Game.Shared.Models
{
    public class Resources
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The Wood cost {0} must be greater than {1}.")]
        public int Wood { get; set; }      

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The Stone cost {0} must be greater than {1}.")]
        public int Stone { get; set; }       

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The Silver cost {0} must be greater than {1}.")]
        public int Silver { get; set; }      

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The Population cost {0} must be greater than {1}.")]
        public int Population { get; set; }
    }
}
