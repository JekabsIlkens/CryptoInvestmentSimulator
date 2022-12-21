using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
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
        private static readonly UserProcedures userProcedures = new(context);
        private static readonly WalletProcedures walletProcedures = new(context);
        private static readonly MarketDataProcedures marketProcedures = new(context);

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
            ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);

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
            userProcedures.UpdateUsername(email, username);
            userProcedures.UpdateAvatar(email, avatar);
            userProcedures.UpdateTimeZone(email, DbKeyConversionHelper.TimeZoneToDbKey(timezone));

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
            return userProcedures.GetUserDetails(email);
        }

        /// <summary>
        /// Converts wallet balances into a percent split of total value in wallets.
        /// </summary>
        /// <param name="userId">Wallet owner</param>
        /// <returns>Wallet percentage split array</returns>
        private decimal[] GetWalletPercentageSplit(int userId)
        {
            var walletBalances = walletProcedures.GetUsersWalletBalances(userId);
            var btcLatest = marketProcedures.GetLatestMarketData(CryptoEnum.BTC);
            var ethLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ETH);
            var adaLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ADA);
            var atomLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ATOM);
            var dogeLatest = marketProcedures.GetLatestMarketData(CryptoEnum.DOGE);

            decimal[] allToEuro = new decimal[6];

            allToEuro[0] = walletBalances.EuroAmount;
            allToEuro[1] = btcLatest.UnitValue * walletBalances.BitcoinAmount;
            allToEuro[2] = ethLatest.UnitValue * walletBalances.EtheriumAmount;
            allToEuro[3] = adaLatest.UnitValue * walletBalances.CardanoAmount;
            allToEuro[4] = atomLatest.UnitValue * walletBalances.CosmosAmount;
            allToEuro[5] = dogeLatest.UnitValue * walletBalances.DogecoinAmount;

            var hundredPercent = 0M;
            for (int i = 0; i < allToEuro.Length; i++) hundredPercent += allToEuro[i];

            decimal[] allToPercent = new decimal[6];

            allToPercent[0] = (allToEuro[0] * 100) / hundredPercent;
            allToPercent[1] = (allToEuro[1] * 100) / hundredPercent;
            allToPercent[2] = (allToEuro[2] * 100) / hundredPercent;
            allToPercent[3] = (allToEuro[3] * 100) / hundredPercent;
            allToPercent[4] = (allToEuro[4] * 100) / hundredPercent;
            allToPercent[5] = (allToEuro[5] * 100) / hundredPercent;

            return allToPercent;
        }
    }
}
