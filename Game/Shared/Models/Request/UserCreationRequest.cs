using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Request
{
    public class UserCreationRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }     
        
        [Required]
        [StringLength(50, MinimumLength = 6)]        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,50}$",
            ErrorMessage = "Password must contain at least 1 upper and lower case character, a digit and a special character")]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]         
        [Compare(nameof(Password), ErrorMessage = "The confirmation does not match the password")]
        public string ConfirmPassword { get; set; }


        
    }
}
