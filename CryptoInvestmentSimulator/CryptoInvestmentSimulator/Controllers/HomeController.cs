using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CryptoInvestmentSimulator.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            var user = new UserModel()
            {
                FirstName = User.FindFirst(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = User.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value,
                EmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                AvatarUrl = User.FindFirst(c => c.Type == "picture")?.Value
            };

            var context = new DatabaseContext(DatabaseConstants.Access);
            var procedure = new UserProcedures(context);

            procedure.InsertNewUser(user);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}