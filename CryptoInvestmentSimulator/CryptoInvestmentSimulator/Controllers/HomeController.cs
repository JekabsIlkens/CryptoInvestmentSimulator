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
            NewUserCheck();
            EmailVerificationCheck();
            return View();
        }

        [HttpPost]
        public IActionResult SetUsername(string username)
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var context = new DatabaseContext(DatabaseConstants.Access);
            var procedure = new UserProcedures(context);

            procedure.UpdateUsername(email, username);
            return View("Index");
        }

        public string GetUsername()
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var context = new DatabaseContext(DatabaseConstants.Access);
            var procedure = new UserProcedures(context);
            var user = procedure.GetUserDetails(email);
            return user.Username;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void NewUserCheck()
        {
            var user = new UserModel()
            {
                Username = "TestName",
                Email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                AvatarUrl = User.FindFirst(c => c.Type == "picture")?.Value,
                IsVerified = false
            };

            var context = new DatabaseContext(DatabaseConstants.Access);
            var procedure = new UserProcedures(context);

            procedure.InsertNewUser(user);
        }

        private void EmailVerificationCheck()
        {
            var claims = User.Identities.First().Claims;
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null || email == "")
            {
                throw new ArgumentNullException(nameof(email));
            }

            foreach (var claim in claims)
            {
                if (claim.Type == "email_verified" && claim.Value == "true")
                {
                    var context = new DatabaseContext(DatabaseConstants.Access);
                    var procedure = new UserProcedures(context);

                    procedure.UpdateVerification(email);
                }

            }
        }
    }
}