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
        private static readonly InvestmentProcedures investmentProcedures = new(context);

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

            ViewBag.TotalEarnings = GetFullPortfolioValueEuro() - 5000M;

            ViewBag.PNL = GetProfitAndLossMeasurement();

            ViewBag.CryptoAmounts = GetWalletTableCryptoAmounts();
            ViewBag.CryptosToEuro = GetWalletTableCryptosToEuro();
            ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);

            ViewBag.Symbols = GetInvestmentTableSymbols();
            ViewBag.DateTimes = GetInvestmentTableDateTimes();
            ViewBag.FiatAmounts = GetInvestmentTableFiatAmounts();
            ViewBag.CryptoAmounts = GetInvestmentTableCryptoAmounts();
            ViewBag.Ratios = GetInvestmentTableRatios();
            ViewBag.Margins = GetInvestmentTableMargins();

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
            var user = GetUserDetails();

            userProcedures.UpdateUsername(user.Email, username);
            userProcedures.UpdateAvatar(user.Email, avatar);
            userProcedures.UpdateTimeZone(user.Email, DbKeyConversionHelper.TimeZoneToDbKey(timezone));

            user = GetUserDetails();
            ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);
            ViewBag.TotalEarnings = GetFullPortfolioValueEuro() - 5000M;

            return View("Index", user);
        }

        /// <summary>
        /// Performs confidence key match check and resets users portfolio.
        /// </summary>
        /// <param name="actualKey">Expected key</param>
        /// <param name="receivedKey">User input</param>
        /// <returns>Portfolio view with user view model</returns>
        [HttpPost]
        public IActionResult ResetPortfolio(string actualKey, string receivedKey)
        {
            var user = GetUserDetails();

            if (actualKey == receivedKey)
            {
                // TODO: Add investment clear procedure
                ResetUsersWallets(user.Id);

                ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);

                return View("Index", user);
            }

            ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);

            return View("Index", user);
        }

        public IActionResult WalletTable()
        {
            var userId = GetUserDetails().Id;

            ViewBag.CryptoAmounts = GetWalletTableCryptoAmounts();
            ViewBag.CryptosToEuro = GetWalletTableCryptosToEuro();
            ViewBag.WalletPercent = GetWalletPercentageSplit(userId);

            ViewBag.EuroAmount = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());

            return PartialView("_WalletTable");
        }

        public IActionResult InvestmentTable()
        {
            var userId = GetUserDetails().Id;

            ViewBag.Symbols = GetInvestmentTableSymbols();
            ViewBag.DateTimes = GetInvestmentTableDateTimes();
            ViewBag.FiatAmounts = GetInvestmentTableFiatAmounts();
            ViewBag.CryptoAmounts = GetInvestmentTableCryptoAmounts();
            ViewBag.Ratios = GetInvestmentTableRatios();
            ViewBag.Margins = GetInvestmentTableMargins();
            ViewBag.Profits = GetOpenPositionUnrealizedProfits();

            return PartialView("_InvestmentTable");
        }

        private decimal[] GetOpenPositionUnrealizedProfits()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);

            decimal[] potentialProfitsList = new decimal[positionsList.Count];

            var count = 0;
            foreach(var position in positionsList)
            {
                var buyTimeUnitValue = investmentProcedures.GetPositionsUnitValue(position.Id);
                var currentUnitValue = marketProcedures.GetLatestMarketData(IntToCryptoEnum(position.BoughtCrypto)).UnitValue;

                if (position.Leverage == 2)
                {
                    potentialProfitsList[count] = (currentUnitValue - buyTimeUnitValue) * 2 * position.FiatAmount;
                }
                else if (position.Leverage == 3)
                {
                    potentialProfitsList[count] = (currentUnitValue - buyTimeUnitValue) * 5 * position.FiatAmount;
                }
                else if (position.Leverage == 4)
                {
                    potentialProfitsList[count] = (currentUnitValue - buyTimeUnitValue) * 10 * position.FiatAmount;
                }
                else
                {
                    potentialProfitsList[count] = (currentUnitValue - buyTimeUnitValue) * position.FiatAmount;
                }
            }

            return potentialProfitsList;
        }

        private decimal GetProfitAndLossMeasurement()
        {
            var profitsList = GetOpenPositionUnrealizedProfits();
            var profitSum = 0M;

            foreach(var profit in profitsList)
            {
                profitSum += profit;
            }

            var PNL = (GetFullPortfolioValueEuro() - 5000M) + (GetFullPortfolioValueEuro() - 5000M) + profitSum;

            return PNL;
        }

        private decimal[] GetWalletTableCryptoAmounts()
        {
            var userId = GetUserDetails().Id;
            var walletBalances = walletProcedures.GetUsersWalletBalances(userId);

            decimal[] cryptoAmounts = new decimal[5];
            cryptoAmounts[0] = walletBalances.BitcoinAmount;
            cryptoAmounts[1] = walletBalances.EtheriumAmount;
            cryptoAmounts[2] = walletBalances.CardanoAmount;
            cryptoAmounts[3] = walletBalances.CosmosAmount;
            cryptoAmounts[4] = walletBalances.DogecoinAmount;

            return cryptoAmounts;
        }

        private decimal[] GetWalletTableCryptosToEuro()
        {
            var userId = GetUserDetails().Id;
            var walletBalances = walletProcedures.GetUsersWalletBalances(userId);

            var btcLatest = marketProcedures.GetLatestMarketData(CryptoEnum.BTC);
            var ethLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ETH);
            var adaLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ADA);
            var atomLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ATOM);
            var dogeLatest = marketProcedures.GetLatestMarketData(CryptoEnum.DOGE);

            decimal[] cryptosToEuro = new decimal[5];
            cryptosToEuro[0] = walletBalances.BitcoinAmount * btcLatest.UnitValue;
            cryptosToEuro[1] = walletBalances.EtheriumAmount * ethLatest.UnitValue;
            cryptosToEuro[2] = walletBalances.CardanoAmount * adaLatest.UnitValue;
            cryptosToEuro[3] = walletBalances.CosmosAmount * atomLatest.UnitValue;
            cryptosToEuro[4] = walletBalances.DogecoinAmount * dogeLatest.UnitValue;

            return cryptosToEuro;
        }

        private string[] GetInvestmentTableSymbols()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);
            var length = positionsList.Count;

            string[] symbols = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                symbols[count] = DbKeyConversionHelper.CryptoKeyToSymbol(position.BoughtCrypto);
                count++;
            }

            return symbols;
        }

        private string[] GetInvestmentTableDateTimes()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);
            var length = positionsList.Count;

            string[] dateTimes = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                dateTimes[count] = DateTimeFormatHelper.ToDbFormatAsString(position.DateTime);
                count++;
            }

            return dateTimes;
        }

        private string[] GetInvestmentTableFiatAmounts()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);
            var length = positionsList.Count;

            string[] fiatAmounts = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                fiatAmounts[count] = position.FiatAmount.ToString();
                count++;
            }

            return fiatAmounts;
        }

        private string[] GetInvestmentTableCryptoAmounts()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);
            var length = positionsList.Count;

            string[] cryptoAmounts = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                cryptoAmounts[count] = position.CryptoAmount.ToString();
                count++;
            }

            return cryptoAmounts;
        }

        private string[] GetInvestmentTableRatios()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);
            var length = positionsList.Count;

            string[] ratios = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                ratios[count] = DbKeyConversionHelper.LeverageKeyToString(position.Leverage);
                count++;
            }

            return ratios;
        }

        private string[] GetInvestmentTableMargins()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);
            var length = positionsList.Count;

            string[] margins = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                margins[count] = position.Margin.ToString();
                count++;
            }

            return margins;
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
            allToEuro[1] = walletBalances.BitcoinAmount * btcLatest.UnitValue;
            allToEuro[2] = walletBalances.EtheriumAmount * ethLatest.UnitValue;
            allToEuro[3] = walletBalances.CardanoAmount * adaLatest.UnitValue;
            allToEuro[4] = walletBalances.CosmosAmount * atomLatest.UnitValue;
            allToEuro[5] = walletBalances.DogecoinAmount * dogeLatest.UnitValue;

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

        private decimal GetFullPortfolioValueEuro()
        {
            var userId = GetUserDetails().Id;

            var walletBalances = walletProcedures.GetUsersWalletBalances(userId);
            var btcLatest = marketProcedures.GetLatestMarketData(CryptoEnum.BTC);
            var ethLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ETH);
            var adaLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ADA);
            var atomLatest = marketProcedures.GetLatestMarketData(CryptoEnum.ATOM);
            var dogeLatest = marketProcedures.GetLatestMarketData(CryptoEnum.DOGE);

            decimal fullValue = 0M;

            fullValue += walletBalances.EuroAmount;
            fullValue += walletBalances.BitcoinAmount * btcLatest.UnitValue;
            fullValue += walletBalances.EtheriumAmount * ethLatest.UnitValue;
            fullValue += walletBalances.CardanoAmount * adaLatest.UnitValue;
            fullValue += walletBalances.CosmosAmount * atomLatest.UnitValue;
            fullValue += walletBalances.DogecoinAmount * dogeLatest.UnitValue;

            return fullValue;
        }

        /// <summary>
        /// Resets all wallet balances for a given user to initial amounts.
        /// </summary>
        /// <param name="userId">Target user</param>
        private void ResetUsersWallets(int userId)
        {
            walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), BusinessRuleConstants.InitialCapital);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.BTC.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ETH.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ADA.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ATOM.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.DOGE.ToString(), 0);
        }

        private CryptoEnum IntToCryptoEnum(int symbolKey)
        {
            return symbolKey switch
            {
                1 => CryptoEnum.BTC,
                2 => CryptoEnum.ETH,
                3 => CryptoEnum.ADA,
                4 => CryptoEnum.ATOM,
                5 => CryptoEnum.DOGE,
                _ => throw new ArgumentException(nameof(symbolKey))
            };
        }
    }
}
