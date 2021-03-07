using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackEnd.Models.Models
{
    public class ApplicationUser : IdentityUser
    {     
        public List<City> Cities { get; set; } = new List<City>();

        public bool IsBanned { get; set; } = false;

    }
}
