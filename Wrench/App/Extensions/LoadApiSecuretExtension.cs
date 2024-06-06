using App.Extensions.StartupHandles.ApiSecurets;
using App.Settings;
using App.Settings.ApiSecurets.Types;
using Microsoft.AspNetCore.Authentication;

namespace App.Extensions
{
    public static class LoadApiSecuretExtension
    {
        private static bool isStarted = false;
        public static void AddApiSecuret(this WebApplicationBuilder builder)
        {
            SetToBasicAuthenticate(builder);
        }

        public static void UseConfigAuthentication(this WebApplication app)
        {
            if (isStarted)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
        }

        private static void SetToBasicAuthenticate(this WebApplicationBuilder builder)
        {
            if (ApplicationSetting.Current?.Startup?.ApiSecurity?.Type == ApiSecurityType.Basic)
            {
                new BasicCredential();

                builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
                builder.Services.AddAuthorization();
                isStarted = true;
            }
        }
    }
}
