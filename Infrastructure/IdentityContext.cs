namespace BackEnd.Infrastructure
{
    public class IdentityContext : IIdentityContext
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}
