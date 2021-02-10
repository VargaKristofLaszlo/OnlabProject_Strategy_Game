using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Request
{
    public class EmailUpdateConfirmationRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string NewEmail { get; set; }

        [Required]
        public string Token { get; set; }
      
    }
}
