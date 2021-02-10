using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Request
{
    public class EmailUpdateRequest
    {    

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string newEmail { get; set; }
    }
}
