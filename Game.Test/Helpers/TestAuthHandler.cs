using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Game.Test.Helpers
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "test_id"),
                new Claim(ClaimTypes.Name, "Test_user_name"),
                new Claim(ClaimTypes.Email, "Test@email.hu"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("amr", "[pwd]"),
                new Claim("auth_time",  "1632859724" ),
                new Claim("idp", "local"),
                new Claim("name", "kristof"),
                new Claim("preferred_username", "kristof"),
                new Claim("role", "User"),
                new Claim("s_hash", "Bt3xO5npbuBJH3hw9QyOuA"),
                new Claim("sid", "0BB2261CD5F4A4DB2D5E2A7E3C9AAE6C"),
                new Claim("sub", "e609b776-cfff-45a3-93a7-2dda45ff0a5f")
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
