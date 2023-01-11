using Microsoft.AspNetCore.Mvc;

namespace CryptoInvestmentSimulator.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
