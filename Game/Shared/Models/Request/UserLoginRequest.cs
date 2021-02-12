using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Request
{
    public class UserLoginRequest
    {
        [Required]    
        public string Username { get; set; }


        [Required]        
        public string Password { get; set; }
    }
}
