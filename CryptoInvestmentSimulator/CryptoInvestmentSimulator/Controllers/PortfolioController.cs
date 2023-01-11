using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Helpers;
using CryptoInvestmentSimulator.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Index view for portfolio page.
        /// </summary>
        /// <returns>
        /// Portfolio view with filled <see cref="UserModel"/> and statistics view bags.
        /// </returns>
        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var user = GetUserDetails();

                // Data for wallet percentage split diagram.
                ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);

                // Additional statistics parameters.
                ViewBag.TotalEarnings = GetFullPortfolioValueEuro() - 5000M;
                ViewBag.PNL = GetProfitAndLossMeasurement();

                // Data for balance amounts table.
                ViewBag.CryptoAmounts = GetWalletTableCryptoAmounts();
                ViewBag.CryptosToEuro = GetWalletTableCryptosToEuro();
                ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);

                // Data for active investments table.
                ViewBag.Symbols = GetInvestmentTableSymbols();
                ViewBag.DateTimes = GetInvestmentTableDateTimes();
                ViewBag.FiatAmounts = GetInvestmentTableFiatAmounts();
                ViewBag.CryptoAmounts = GetInvestmentTableCryptoAmounts();
                ViewBag.Ratios = GetInvestmentTableRatios();
                ViewBag.Margins = GetInvestmentTableMargins();

                return View("Index", user);
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Receives data trough from user details editing form.
        /// Performs username, avatar and time zone update procedures.
        /// </summary>
        /// <param name="username">New username.</param>
        /// <param name="avatar">New avatar url.</param>
        /// <param name="timezone">New selected time zone.</param>
        /// <returns>
        /// Portfolio view with filled <see cref="UserModel"/> and statistics view bags.
        /// </returns>
        [HttpPost]
        public IActionResult UpdateDetails(string username, string avatar, string timezone)
        {
            try
            {
                var userId = GetUserDetails().Id;

                var timeZoneKey = DatabaseKeyConversionHelper.TimeZoneToDbKey(timezone);
                userProcedures.UpdateUsername(userId, username);
                userProcedures.UpdateAvatar(userId, avatar);
                userProcedures.UpdateTimeZone(userId, timeZoneKey);

                var user = GetUserDetails();
                ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);
                ViewBag.TotalEarnings = GetFullPortfolioValueEuro() - 5000M;
                ViewBag.PNL = GetProfitAndLossMeasurement();

                return View("Index", user);
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Gets activated trough reset portfolio form with POST.
        /// Resets users wallet balances to initial values and deletes all owned positions.
        /// </summary>
        /// <returns>
        /// Portfolio view with filled <see cref="UserModel"/> and statistics view bags.
        /// </returns>
        [HttpPost]
        public IActionResult ResetPortfolio()
        {
            try
            {
                var user = GetUserDetails();

                ResetUsersWallets(user.Id);
                investmentProcedures.DeleteUsersPositions(user.Id);

                ViewBag.WalletPercent = GetWalletPercentageSplit(user.Id);
                ViewBag.TotalEarnings = GetFullPortfolioValueEuro() - 5000M;
                ViewBag.PNL = GetProfitAndLossMeasurement();

                return View("Index", user);
            }
            catch
            {
                return View("Error");
            }

        }

        /// <summary>
        /// Fills view bags with necessary wallet table column data.
        /// </summary>
        /// <returns>
        /// _WalletTable partial view.
        /// </returns>
        public IActionResult WalletTable()
        {
            try
            {
                var userId = GetUserDetails().Id;

                ViewBag.CryptoAmounts = GetWalletTableCryptoAmounts();
                ViewBag.CryptosToEuro = GetWalletTableCryptosToEuro();
                ViewBag.WalletPercent = GetWalletPercentageSplit(userId);
                ViewBag.EuroAmount = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());

                return PartialView("_WalletTable");
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Fills view bags with necessary investment table column data.
        /// </summary>
        /// <returns>
        /// _InvestmentTable partial view.
        /// </returns>
        public IActionResult InvestmentTable()
        {
            try
            {
                ViewBag.Symbols = GetInvestmentTableSymbols();
                ViewBag.DateTimes = GetInvestmentTableDateTimes();
                ViewBag.FiatAmounts = GetInvestmentTableFiatAmounts();
                ViewBag.CryptoAmounts = GetInvestmentTableCryptoAmounts();
                ViewBag.Ratios = GetInvestmentTableRatios();
                ViewBag.Margins = GetInvestmentTableMargins();
                ViewBag.Profits = GetOpenPositionUnrealizedProfits();

                return PartialView("_InvestmentTable");
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Gets users all open positions and calculates potential profits
        /// if all open trades were to be closet at given moment.
        /// </summary>
        /// <returns>
        /// Array of all unrealized profits.
        /// </returns>
        private decimal[] GetOpenPositionUnrealizedProfits()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);

            decimal[] potentialProfitsList = new decimal[positionsList.Count];

            var count = 0;
            foreach(var position in positionsList)
            {
                var cryptoEnum = InternalConversionHelper.IntToCryptoEnum(position.BoughtCrypto);
                var buyTimeUnitValue = investmentProcedures.GetPositionsUnitValue(position.Id);
                var currentUnitValue = marketProcedures.GetLatestMarketData(cryptoEnum).UnitValue;

                if (position.Leverage == (int)LeverageEnum.Two)
                {
                    potentialProfitsList[count] = (currentUnitValue - buyTimeUnitValue) * 2 * position.FiatAmount;
                }
                else if (position.Leverage == (int)LeverageEnum.Five)
                {
                    potentialProfitsList[count] = (currentUnitValue - buyTimeUnitValue) * 5 * position.FiatAmount;
                }
                else if (position.Leverage == (int)LeverageEnum.Ten)
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

        /// <summary>
        /// Using users unrealized profits for all open positions and full portfolio value,
        /// calculates the PNL (Profit & Loss) parameter.
        /// </summary>
        /// <returns>
        /// Users PNL measurement.
        /// </returns>
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

        /// <summary>
        /// Collects users crypto wallet balances into decimal array
        /// for use in _WalletsTable partial view.
        /// </summary>
        /// <returns>
        /// Decimal array with users crypto wallet balances.
        /// </returns>
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

        /// <summary>
        /// Converts users crypto wallet balances into euro value
        /// and creates decimal array for use in _WalletsTable partial view.
        /// </summary>
        /// <returns>
        /// Decimal array with users crypto wallet balances as euro.
        /// </returns>
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

        /// <summary>
        /// Collects users open position crypto symbols into string array
        /// for use in _InvestmentTable partial view.
        /// </summary>
        /// <returns>
        /// String array with users open position crypto symbols.
        /// </returns>
        private string[] GetInvestmentTableSymbols()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);

            var length = positionsList.Count;
            string[] symbols = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                symbols[count] = DatabaseKeyConversionHelper.CryptoKeyToSymbol(position.BoughtCrypto);
                count++;
            }

            return symbols;
        }

        /// <summary>
        /// Collects users open position date times into string array
        /// for use in _InvestmentTable partial view.
        /// </summary>
        /// <returns>
        /// String array with users open position date times.
        /// </returns>
        private string[] GetInvestmentTableDateTimes()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);

            var length = positionsList.Count;
            string[] dateTimes = new string[length];

            var usersTimeZoneChange = InternalConversionHelper.TimeZoneStringToChangeValue(GetUserDetails().TimeZone);

            var count = 0;
            foreach (var position in positionsList)
            {
                dateTimes[count] = DateTimeFormatHelper.ToDbFormatAsString(position.DateTime.AddHours(usersTimeZoneChange + 2));
                count++;
            }

            return dateTimes;
        }

        /// <summary>
        /// Collects users open position fiat amounts into string array
        /// for use in _InvestmentTable partial view.
        /// </summary>
        /// <returns>
        /// String array with users open position fiat amounts.
        /// </returns>
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

        /// <summary>
        /// Collects users open position crypto amounts into string array
        /// for use in _InvestmentTable partial view.
        /// </summary>
        /// <returns>
        /// String array with users open position crypto amounts.
        /// </returns>
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

        /// <summary>
        /// Collects users open position selected leverage ratios into string array
        /// for use in _InvestmentTable partial view.
        /// </summary>
        /// <returns>
        /// String array with users open position leverage ratios.
        /// </returns>
        private string[] GetInvestmentTableRatios()
        {
            var userId = GetUserDetails().Id;
            var positionsList = investmentProcedures.GetAllOpenPositions(userId);

            var length = positionsList.Count;
            string[] ratios = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                ratios[count] = DatabaseKeyConversionHelper.LeverageKeyToString(position.Leverage);
                count++;
            }

            return ratios;
        }

        /// <summary>
        /// Collects users open position margins amounts into string array
        /// for use in _InvestmentTable partial view.
        /// </summary>
        /// <returns>
        /// String array with users open position margin amounts.
        /// </returns>
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
        /// Using users wallet balances and latest unit value for each cryptocurrency,
        /// calculates how much percent each asset takes up of full portfolio value.
        /// </summary>
        /// <param name="userId">Wallet owner.</param>
        /// <returns>
        /// Wallet percentage split array.
        /// </returns>
        private static decimal[] GetWalletPercentageSplit(int userId)
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

        /// <summary>
        /// Using users wallet balances and latest unit value for each cryptocurrency,
        /// calculates the total value of a users portfolio.
        /// </summary>
        /// <returns>
        /// Users full portfolio value.
        /// </returns>
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
        /// <param name="userId">Target user.</param>
        private static void ResetUsersWallets(int userId)
        {
            walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), BusinessRuleConstants.InitialCapital);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.BTC.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ETH.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ADA.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ATOM.ToString(), 0);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.DOGE.ToString(), 0);
        }

        /// <summary>
        /// Fills a <see cref="UserModel"/> with current users data.
        /// </summary>
        /// <returns>
        /// Filled <see cref="UserModel"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        private UserModel GetUserDetails()
        {
            var email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null || string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return userProcedures.GetUserDetails(email);
        }
    }
}
