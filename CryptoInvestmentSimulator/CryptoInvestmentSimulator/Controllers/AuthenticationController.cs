using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CryptoInvestmentSimulator.Constants;

namespace CryptoInvestmentSimulator.Controllers
{
    public class AuthenticationController : Controller
    {
        /// <summary>
        /// Triggers the authentication process.
        /// </summary>
        /// <returns>Home page view</returns>
        public async Task Login()
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(AuthenticationConstants.LoginRedirect)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        /// <summary>
        /// Triggers the logout process by destroying Auth0 and local session.
        /// </summary>
        /// <returns>Landing page view</returns>
        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(AuthenticationConstants.LogoutRedirect)
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
