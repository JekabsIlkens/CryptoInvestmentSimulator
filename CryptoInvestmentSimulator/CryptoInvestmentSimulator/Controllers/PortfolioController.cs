using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Helpers;
using CryptoInvestmentSimulator.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CryptoInvestmentSimulator.Controllers
{
    public class PortfolioController : Controller
    {
        private static readonly DatabaseContext context = new(DatabaseConstants.Access);
        private static readonly UserProcedures procedures = new(context);

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Index view for portfolio page.
        /// </summary>
        /// <returns>Portfolio view with user view model</returns>
        [Authorize]
        public IActionResult Index()
        {
            var user = GetUserDetails();

            return View("Index", user);
        }

        /// <summary>
        /// Performs username and avatar update procedures after user
        /// fills in the details editing popup.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="avatar"></param>
        /// <returns>Portfolio view with user view model</returns>
        [HttpPost]
        public IActionResult UpdateDetails(string username, string avatar, string timezone)
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            procedures.UpdateUsername(email, username);
            procedures.UpdateAvatar(email, avatar);
            procedures.UpdateTimeZone(email, DbKeyConversionHelper.TimeZoneToDbKey(timezone));

            var user = GetUserDetails();

            return View("Index", user);
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
