using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Request
{
    public class PasswordUpdateRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Compare(nameof(NewPassword), ErrorMessage = "The confirmation does not match the password")]
        public string ConfirmPassword { get; set; }
    }
}
