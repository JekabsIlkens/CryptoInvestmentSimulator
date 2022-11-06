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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private UserModel GetUserData()
        {
            if (!User.HasClaim(c => c.Type == ClaimTypes.GivenName) ||
                !User.HasClaim(c => c.Type == ClaimTypes.Surname) ||
                !User.HasClaim(c => c.Type == ClaimTypes.Email) || 
                !User.HasClaim(c => c.Type == "picture"))
            {
                throw new ArgumentNullException(nameof(User));
            }


            return new UserModel()
            {
                FirstName = User.FindFirst(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = User.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value,
                EmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                AvatarUrl = User.FindFirst(c => c.Type == "picture")?.Value
            };
        }
    }
}
