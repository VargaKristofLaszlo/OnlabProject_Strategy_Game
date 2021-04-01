using System.Security.Claims;

namespace BackEnd.Infrastructure
{
    public class IdentityContext : IIdentityContext
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public IdentityContext()
        {

        }

        public IdentityContext(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {   
            if (httpContext.User.Identity.IsAuthenticated)
            {
                string id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                string username = httpContext.User.FindFirst(ClaimTypes.Name).Value;
                string email = httpContext.User.FindFirst(ClaimTypes.Email).Value;
                bool isAdmin = "Admin".Equals(httpContext.User.FindFirst(ClaimTypes.Role).Value);

                UserId = id;
                Username = username;
                Email = email;
                IsAdmin = isAdmin;
            }
        }
    }
}
