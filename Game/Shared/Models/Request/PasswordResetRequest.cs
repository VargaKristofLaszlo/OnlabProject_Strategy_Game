using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Request
{
    public class PasswordResetRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]        
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Compare(nameof(NewPassword), ErrorMessage = "The confirmation does not match the password")]
        public string ConfirmPassword { get; set; }

    }
}
