using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CryptoInvestmentSimulator.Controllers
{
    public class HomeController : Controller
    {
        private static readonly DatabaseContext context = new (DatabaseConstants.Access);
        private static readonly UserProcedures procedures = new (context);

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Index view for home page.
        /// Performs new user and email verification checks.
        /// </summary>
        /// <returns>Home view with user view model</returns>
        [Authorize]
        public IActionResult Index()
        {
            NewUserCheck();
            EmailVerificationCheck();
            var user = GetUserDetails();

            return View(user);
        }

        /// <summary>
        /// Performs username update after user fills in the new user popup.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Home view with user view model</returns>
        [HttpPost]
        public IActionResult SetUsername(string username)
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            procedures.UpdateUsername(email, username);

            var user = GetUserDetails();

            return View("Index", user);
        }

        /// <summary>
        /// Creates new user model with default username and verification status.
        /// Performs new user insertion procedure.
        /// </summary>
        private void NewUserCheck()
        {
            var user = new UserModel()
            {
                Username = "NewUser",
                Email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                AvatarUrl = User.FindFirst(c => c.Type == "picture")?.Value,
                IsVerified = false,
                TimeZone = "GMT+02:00"
            };

            procedures.InsertNewUser(user);
        }

        /// <summary>
        /// Compares Auth0 and database verification statuses.
        /// If mismatch - performs verification status update procedure.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void EmailVerificationCheck()
        {
            var claims = User.Identities.First().Claims;
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException($"Received {nameof(email)} is null or empty!");
            }

            foreach (var claim in claims)
            {
                if (claim.Type == "email_verified" && claim.Value == "true")
                {
                    procedures.UpdateVerification(email);
                }

            }
        }

        /// <summary>
        /// Fills a user model for use in other methods.
        /// </summary>
        /// <returns>Filled user model</returns>
        private UserModel GetUserDetails()
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            return procedures.GetUserDetails(email);
        }
    }
}