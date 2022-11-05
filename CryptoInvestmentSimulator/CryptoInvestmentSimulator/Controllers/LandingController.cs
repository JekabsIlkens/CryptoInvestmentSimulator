using CryptoInvestmentSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CryptoInvestmentSimulator.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
