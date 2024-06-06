using App.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace App.Extensions.StartupHandles.ApiSecurets
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var appSettings = ApplicationSetting.Current;
            var authHeader = Request.Headers["Authorization"].ToString();
            var isAuthenticated = BasicCredential.Current.Validate(authHeader);

            if (isAuthenticated)
            {
                var claims = new[] { new Claim("name", BasicCredential.Current.User) };
                var identity = new ClaimsIdentity(claims, "Basic");
                var claimsPrincipal = new ClaimsPrincipal(identity);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
