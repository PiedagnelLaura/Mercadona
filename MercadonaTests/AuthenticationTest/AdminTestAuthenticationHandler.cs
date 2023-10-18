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

namespace MercadonaTests.Authentication
{
    public class AdminTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AdminTestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] {
            new Claim(ClaimTypes.Name, "admin@admin.com"),
            new Claim(ClaimTypes.Role, "Administrator"),
        };

            var identity = new ClaimsIdentity(claims, "AdminTest");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "AdminTest");

            return AuthenticateResult.Success(ticket);
        }
    }
}
