using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BackEnd.Models.Models
{
    public class ApplicationUser : IdentityUser
    {     
        public List<City> Cities { get; set; } = new List<City>();

        public bool IsBanned { get; set; } = false;
    }
}
