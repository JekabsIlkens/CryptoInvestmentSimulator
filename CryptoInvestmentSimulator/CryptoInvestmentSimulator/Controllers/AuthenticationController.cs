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
        /// Triggers the authentication process forwarding user to login page.
        /// After authentication sends user to the home page.
        /// </summary>
        public async Task Login()
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(AuthenticationConstants.LoginRedirect)
                .WithParameter("screen_hint", "signin")
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        /// <summary>
        /// Triggers the authentication register process forwarding user to registration page.
        /// After authentication sends user to the home page.
        /// </summary>
        public async Task Register()
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(AuthenticationConstants.LoginRedirect)
                .WithParameter("screen_hint", "signup")
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        /// <summary>
        /// Triggers the logout process by destroying Auth0 and local session.
        /// After logout sends user to the landing page.
        /// </summary>
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
