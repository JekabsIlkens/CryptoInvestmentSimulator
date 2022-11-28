using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CryptoInvestmentSimulator.Controllers
{
    public class PortfolioController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            var user = GetUserData();
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateDetails(string username, string avatar)
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var context = new DatabaseContext(DatabaseConstants.Access);
            var procedure = new UserProcedures(context);

            procedure.UpdateUserDetails(email, username, avatar);
            var user = GetUserData();
            return View("Index", user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private UserModel GetUserData()
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var context = new DatabaseContext(DatabaseConstants.Access);
            var procedure = new UserProcedures(context);

            return procedure.GetSpecificUser(email);
        }
    }
}
