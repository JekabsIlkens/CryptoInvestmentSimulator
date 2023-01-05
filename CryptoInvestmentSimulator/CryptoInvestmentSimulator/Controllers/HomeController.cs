using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoInvestmentSimulator.Controllers
{
    public class HomeController : Controller
    {
        private static readonly DatabaseContext context = new (DatabaseConstants.Access);
        private static readonly UserProcedures userProcedures = new (context);

        /// <summary>
        /// Index view for home page.
        /// Checks if currently loggen in user is a new user.
        /// Checks if database verification status matches status from Auth0.
        /// </summary>
        /// <returns>
        /// Index view with filled <see cref="UserModel"/>.
        /// </returns>
        [Authorize]
        public IActionResult Index()
        {
            NewUserCheck();
            EmailVerificationCheck();

            var user = GetCurrentUserDetails();

            return View("Index", user);
        }

        /// <summary>
        /// Performs username update after user fills username creation form.
        /// </summary>
        /// <param name="username">Username recieved from input form.</param>
        /// <returns>
        /// Index view with filled <see cref="UserModel"/>.
        /// </returns>
        [HttpPost]
        public IActionResult SetUsername(string username)
        {
            var userId = GetCurrentUserDetails().Id;

            userProcedures.UpdateUsername(userId, username);

            var updatedUser = GetCurrentUserDetails();

            return View("Index", updatedUser);
        }

        /// <summary>
        /// Creates new <see cref="UserModel"/> with default temporary data.
        /// Performs new user insertion procedure that checks for users existance.
        /// </summary>
        private void NewUserCheck()
        {
            var newUser = new UserModel()
            {
                Email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                Verified = BusinessRuleConstants.InitialVerificationStatus,
                Username = BusinessRuleConstants.InitialUsername,
                Avatar = BusinessRuleConstants.InitialAvatarImageUrl,
                TimeZone = BusinessRuleConstants.InitialTimeZone
            };

            userProcedures.InsertNewUser(newUser);
        }

        /// <summary>
        /// Compares Auth0 and database verification statuses.
        /// If mismatch - performs verification status update procedure.
        /// </summary>
        private void EmailVerificationCheck()
        {
            var user = GetCurrentUserDetails();
            var claims = User.Identities.First().Claims;

            foreach (var claim in claims)
            {
                if (claim.Type == "email_verified" && claim.Value == "true" && user.Verified == 0)
                {
                    userProcedures.UpdateVerification(user.Id);
                    userProcedures.CreateWalletsForUser(user.Id);
                }

            }
        }

        /// <summary>
        /// Fills a <see cref="UserModel"/> with current users data.
        /// </summary>
        /// <returns>
        /// Filled <see cref="UserModel"/>
        /// </returns>
        private UserModel GetCurrentUserDetails()
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

            return userProcedures.GetUserDetails(email);
        }
    }
}