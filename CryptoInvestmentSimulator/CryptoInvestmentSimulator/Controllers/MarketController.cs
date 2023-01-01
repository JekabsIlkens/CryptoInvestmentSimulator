using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Helpers;
using CryptoInvestmentSimulator.Models.ResponseModels;
using CryptoInvestmentSimulator.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;

namespace CryptoInvestmentSimulator.Controllers
{
    public class MarketController : Controller
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

        [Authorize]
        public IActionResult Index()
        {
            return View("Index");
        }

        [Authorize]
        public IActionResult Bitcoin()
        {
            return View("Bitcoin");
        }

        [Authorize]
        public IActionResult Etherium()
        {
            return View("Etherium");
        }

        [Authorize]
        public IActionResult Cardano()
        {
            return View("Cardano");
        }

        [Authorize]
        public IActionResult Cosmos()
        {
            return View("Cosmos");
        }

        [Authorize]
        public IActionResult Dogecoin()
        {
            return View("Dogecoin");
        }

        /// <summary>
        /// Partial view for market page data table.
        /// Gets list of lates marked data view models for each crypto.
        /// Passes list to ViewBag for dynamic data display.
        /// </summary>
        /// <returns>_DataTable partial view with filled ViewBag</returns>
        [Authorize]
        public IActionResult DataTable()
        {
            ViewBag.MarketData = GetLatestMarketRecords();

            return PartialView("_DataTable");
        }

        /// <summary>
        /// Prepares view bags with users open BTC position data for each table column.
        /// </summary>
        /// <returns>Positions table partial view</returns>
        [Authorize]
        public IActionResult PositionsTableBTC()
        {
            var positionsList = investmentProcedures.GetAllActivePositions(GetUserDetails().Id, CryptoEnum.BTC);
            var length = positionsList.Count;

            string[] dateTimes = new string[length];
            string[] fiatAmounts = new string[length];
            string[] cryptoAmounts = new string[length];
            string[] ratios = new string[length];
            string[] margins = new string[length];

            var count = 0;
            foreach(var position in positionsList)
            {
                dateTimes[count] = DateTimeFormatHelper.ToDbFormatAsString(position.DateTime);
                fiatAmounts[count] = position.FiatAmount.ToString();
                cryptoAmounts[count] = position.CryptoAmount.ToString();
                ratios[count] = DbKeyConversionHelper.LeverageKeyToString(position.Leverage);
                margins[count] = position.Margin.ToString();

                count++;
            }

            ViewBag.DateTimes = dateTimes;
            ViewBag.FiatAmounts = fiatAmounts;
            ViewBag.CryptoAmounts = cryptoAmounts;
            ViewBag.Ratios = ratios;
            ViewBag.Margins = margins;

            return PartialView("_PositionsTable");
        }

        /// <summary>
        /// Prepares view bags with users open ETH position data for each table column.
        /// </summary>
        /// <returns>Positions table partial view</returns>
        [Authorize]
        public IActionResult PositionsTableETH()
        {
            var positionsList = investmentProcedures.GetAllActivePositions(GetUserDetails().Id, CryptoEnum.ETH);
            var length = positionsList.Count;

            string[] dateTimes = new string[length];
            string[] fiatAmounts = new string[length];
            string[] cryptoAmounts = new string[length];
            string[] ratios = new string[length];
            string[] margins = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                dateTimes[count] = DateTimeFormatHelper.ToDbFormatAsString(position.DateTime);
                fiatAmounts[count] = position.FiatAmount.ToString();
                cryptoAmounts[count] = position.CryptoAmount.ToString();
                ratios[count] = DbKeyConversionHelper.LeverageKeyToString(position.Leverage);
                margins[count] = position.Margin.ToString();

                count++;
            }

            ViewBag.DateTimes = dateTimes;
            ViewBag.FiatAmounts = fiatAmounts;
            ViewBag.CryptoAmounts = cryptoAmounts;
            ViewBag.Ratios = ratios;
            ViewBag.Margins = margins;

            return PartialView("_PositionsTable");
        }

        /// <summary>
        /// Prepares view bags with users open ADA position data for each table column.
        /// </summary>
        /// <returns>Positions table partial view</returns>
        [Authorize]
        public IActionResult PositionsTableADA()
        {
            var positionsList = investmentProcedures.GetAllActivePositions(GetUserDetails().Id, CryptoEnum.ADA);
            var length = positionsList.Count;

            string[] dateTimes = new string[length];
            string[] fiatAmounts = new string[length];
            string[] cryptoAmounts = new string[length];
            string[] ratios = new string[length];
            string[] margins = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                dateTimes[count] = DateTimeFormatHelper.ToDbFormatAsString(position.DateTime);
                fiatAmounts[count] = position.FiatAmount.ToString();
                cryptoAmounts[count] = position.CryptoAmount.ToString();
                ratios[count] = DbKeyConversionHelper.LeverageKeyToString(position.Leverage);
                margins[count] = position.Margin.ToString();

                count++;
            }

            ViewBag.DateTimes = dateTimes;
            ViewBag.FiatAmounts = fiatAmounts;
            ViewBag.CryptoAmounts = cryptoAmounts;
            ViewBag.Ratios = ratios;
            ViewBag.Margins = margins;

            return PartialView("_PositionsTable");
        }

        /// <summary>
        /// Prepares view bags with users open ATOM position data for each table column.
        /// </summary>
        /// <returns>Positions table partial view</returns>
        [Authorize]
        public IActionResult PositionsTableATOM()
        {
            var positionsList = investmentProcedures.GetAllActivePositions(GetUserDetails().Id, CryptoEnum.ATOM);
            var length = positionsList.Count;

            string[] dateTimes = new string[length];
            string[] fiatAmounts = new string[length];
            string[] cryptoAmounts = new string[length];
            string[] ratios = new string[length];
            string[] margins = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                dateTimes[count] = DateTimeFormatHelper.ToDbFormatAsString(position.DateTime);
                fiatAmounts[count] = position.FiatAmount.ToString();
                cryptoAmounts[count] = position.CryptoAmount.ToString();
                ratios[count] = DbKeyConversionHelper.LeverageKeyToString(position.Leverage);
                margins[count] = position.Margin.ToString();

                count++;
            }

            ViewBag.DateTimes = dateTimes;
            ViewBag.FiatAmounts = fiatAmounts;
            ViewBag.CryptoAmounts = cryptoAmounts;
            ViewBag.Ratios = ratios;
            ViewBag.Margins = margins;

            return PartialView("_PositionsTable");
        }

        /// <summary>
        /// Prepares view bags with users open DOGE position data for each table column.
        /// </summary>
        /// <returns>Positions table partial view</returns>
        [Authorize]
        public IActionResult PositionsTableDOGE()
        {
            var positionsList = investmentProcedures.GetAllActivePositions(GetUserDetails().Id, CryptoEnum.DOGE);
            var length = positionsList.Count;

            string[] dateTimes = new string[length];
            string[] fiatAmounts = new string[length];
            string[] cryptoAmounts = new string[length];
            string[] ratios = new string[length];
            string[] margins = new string[length];

            var count = 0;
            foreach (var position in positionsList)
            {
                dateTimes[count] = DateTimeFormatHelper.ToDbFormatAsString(position.DateTime);
                fiatAmounts[count] = position.FiatAmount.ToString();
                cryptoAmounts[count] = position.CryptoAmount.ToString();
                ratios[count] = DbKeyConversionHelper.LeverageKeyToString(position.Leverage);
                margins[count] = position.Margin.ToString();

                count++;
            }

            ViewBag.DateTimes = dateTimes;
            ViewBag.FiatAmounts = fiatAmounts;
            ViewBag.CryptoAmounts = cryptoAmounts;
            ViewBag.Ratios = ratios;
            ViewBag.Margins = margins;

            return PartialView("_PositionsTable");
        }

        /// <summary>
        /// Collects Bitcoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 1h chart</returns>
        [Authorize]
        public IActionResult BTC1hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.BTC, 60, 1);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.BTC, 60, 1);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart1h");
        }

        /// <summary>
        /// Collects Bitcoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 4h chart</returns>
        [Authorize]
        public IActionResult BTC4hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.BTC, 240, 4);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.BTC, 240, 4);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart4h");
        }

        /// <summary>
        /// Collects Bitcoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 8h chart</returns>
        [Authorize]
        public IActionResult BTC8hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.BTC, 480, 8);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.BTC, 480, 8);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart8h");
        }

        /// <summary>
        /// Collects Bitcoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 24h chart</returns>
        [Authorize]
        public IActionResult BTC24hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.BTC, 1440, 24);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.BTC, 1440, 24);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart24h");
        }

        /// <summary>
        /// Collects Etherium market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 1h chart</returns>
        [Authorize]
        public IActionResult ETH1hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ETH, 60, 1);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ETH, 60, 1);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart1h");
        }

        /// <summary>
        /// Collects Etherium market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 4h chart</returns>
        [Authorize]
        public IActionResult ETH4hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ETH, 240, 4);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ETH, 240, 4);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart4h");
        }

        /// <summary>
        /// Collects Etherium market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 8h chart</returns>
        [Authorize]
        public IActionResult ETH8hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ETH, 480, 8);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ETH, 480, 8);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart8h");
        }

        /// <summary>
        /// Collects Etherium market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 24h chart</returns>
        [Authorize]
        public IActionResult ETH24hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ETH, 1440, 24);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ETH, 1440, 24);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart24h");
        }

        /// <summary>
        /// Collects Cardano market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 1h chart</returns>
        [Authorize]
        public IActionResult ADA1hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ADA, 60, 1);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ADA, 60, 1);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart1h");
        }

        /// <summary>
        /// Collects Cardano market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 4h chart</returns>
        [Authorize]
        public IActionResult ADA4hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ADA, 240, 4);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ADA, 240, 4);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart4h");
        }

        /// <summary>
        /// Collects Cardano market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 8h chart</returns>
        [Authorize]
        public IActionResult ADA8hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ADA, 480, 8);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ADA, 480, 8);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart8h");
        }

        /// <summary>
        /// Collects Cardano market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 24h chart</returns>
        [Authorize]
        public IActionResult ADA24hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ADA, 1440, 24);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ADA, 1440, 24);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart24h");
        }

        /// <summary>
        /// Collects Cosmos market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 1h chart</returns>
        [Authorize]
        public IActionResult ATOM1hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ATOM, 60, 1);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ATOM, 60, 1);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart1h");
        }

        /// <summary>
        /// Collects Cosmos market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 4h chart</returns>
        [Authorize]
        public IActionResult ATOM4hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ATOM, 240, 4);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ATOM, 240, 4);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart4h");
        }

        /// <summary>
        /// Collects Cosmos market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 8h chart</returns>
        [Authorize]
        public IActionResult ATOM8hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ATOM, 480, 8);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ATOM, 480, 8);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart8h");
        }

        /// <summary>
        /// Collects Cosmos market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 24h chart</returns>
        [Authorize]
        public IActionResult ATOM24hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.ATOM, 1440, 24);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.ATOM, 1440, 24);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart24h");
        }

        /// <summary>
        /// Collects Dogecoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 1h chart</returns>
        [Authorize]
        public IActionResult DOGE1hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.DOGE, 60, 1);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.DOGE, 60, 1);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart1h");
        }

        /// <summary>
        /// Collects Dogecoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 4h chart</returns>
        [Authorize]
        public IActionResult DOGE4hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.DOGE, 240, 4);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.DOGE, 240, 4);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart4h");
        }

        /// <summary>
        /// Collects Dogecoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 8h chart</returns>
        [Authorize]
        public IActionResult DOGE8hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.DOGE, 480, 8);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.DOGE, 480, 8);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart8h");
        }

        /// <summary>
        /// Collects Dogecoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 24h chart</returns>
        [Authorize]
        public IActionResult DOGE24hChart()
        {
            var pricePoints = marketProcedures.GetPricePointHistory(CryptoEnum.DOGE, 1440, 24);
            var timePoints = marketProcedures.GetTimePointHistory(CryptoEnum.DOGE, 1440, 24);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart24h");
        }

        /// <summary>
        /// Receives user input form buy form. Validates amounts against users wallet.
        /// If requested purchase amount plus optional margin is larger than euro wallet balance,
        /// then further process is stopped and view bag with error message is prepared.
        /// If requested purchase amount plus optional margin is smaller or equal to euro wallet balance,
        /// then position model is filled, position inserted into DB and wallet balances adjusted acordingly.
        /// </summary>
        /// <param name="euroAmount">Buy amount in euro requested by user</param>
        /// <param name="cryptoAmount">Calculated crypto amount</param>
        /// <param name="leverageRatio">Leverage multiplier selected by user</param>
        /// <param name="marginAmount">Calculated margin amount</param>
        /// <returns>Bitcoin market view</returns>
        [HttpPost]
        public IActionResult OpenBitcoinPosition(string euroAmount, string cryptoAmount, string leverageRatio, string marginAmount)
        {
            var userId = GetUserDetails().Id;
            var currentBalances = walletProcedures.GetUsersWalletBalances(userId);
            var currentEUR = currentBalances.EuroAmount;
            var currentBTC = currentBalances.BitcoinAmount;

            marginAmount = marginAmount ?? "0";
            if (currentEUR < (decimal.Parse(euroAmount) + decimal.Parse(marginAmount)))
            {
                ViewBag.Error = "Insufficient funds for requested purchase!";
                return View("Bitcoin");
            }

            var newPosition = new PositionModel
            {
                DateTime = DateTime.Now,
                FiatAmount = decimal.Parse(euroAmount),
                CryptoAmount = decimal.Parse(cryptoAmount),
                Margin = decimal.Parse(marginAmount),
                Leverage = DbKeyConversionHelper.LeverageStringToDbKey(leverageRatio),
                Status = (int)StatusEnum.Open,
                Wallet = investmentProcedures.GetUserWalletId(userId, FiatEnum.EUR),
                Data = marketProcedures.GetLatestMarketData(CryptoEnum.BTC).Id
            };

            investmentProcedures.InsertNewPosition(newPosition);
            walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), currentEUR - newPosition.FiatAmount);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.BTC.ToString(), currentBTC + newPosition.CryptoAmount);

            return View("Bitcoin");
        }

        /// <summary>
        /// Receives user input form buy form. Validates amounts against users wallet.
        /// If requested purchase amount plus optional margin is larger than euro wallet balance,
        /// then further process is stopped and view bag with error message is prepared.
        /// If requested purchase amount plus optional margin is smaller or equal to euro wallet balance,
        /// then position model is filled, position inserted into DB and wallet balances adjusted acordingly.
        /// </summary>
        /// <param name="euroAmount">Buy amount in euro requested by user</param>
        /// <param name="cryptoAmount">Calculated crypto amount</param>
        /// <param name="leverageRatio">Leverage multiplier selected by user</param>
        /// <param name="marginAmount">Calculated margin amount</param>
        /// <returns>Etherium market view</returns>
        [HttpPost]
        public IActionResult OpenEtheriumPosition(string euroAmount, string cryptoAmount, string leverageRatio, string marginAmount)
        {
            var userId = GetUserDetails().Id;
            var currentBalances = walletProcedures.GetUsersWalletBalances(userId);
            var currentEUR = currentBalances.EuroAmount;
            var currentETH = currentBalances.EtheriumAmount;

            marginAmount = marginAmount ?? "0";
            if (currentEUR < (decimal.Parse(euroAmount) + decimal.Parse(marginAmount)))
            {
                ViewBag.Error = "Insufficient funds for requested purchase!";
                return View("Etherium");
            }

            var newPosition = new PositionModel
            {
                DateTime = DateTime.Now,
                FiatAmount = decimal.Parse(euroAmount),
                CryptoAmount = decimal.Parse(cryptoAmount),
                Margin = decimal.Parse(marginAmount),
                Leverage = DbKeyConversionHelper.LeverageStringToDbKey(leverageRatio),
                Status = (int)StatusEnum.Open,
                Wallet = investmentProcedures.GetUserWalletId(userId, FiatEnum.EUR),
                Data = marketProcedures.GetLatestMarketData(CryptoEnum.ETH).Id
            };

            investmentProcedures.InsertNewPosition(newPosition);
            walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), currentEUR - newPosition.FiatAmount);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ETH.ToString(), currentETH + newPosition.CryptoAmount);

            return View("Etherium");
        }

        /// <summary>
        /// Receives user input form buy form. Validates amounts against users wallet.
        /// If requested purchase amount plus optional margin is larger than euro wallet balance,
        /// then further process is stopped and view bag with error message is prepared.
        /// If requested purchase amount plus optional margin is smaller or equal to euro wallet balance,
        /// then position model is filled, position inserted into DB and wallet balances adjusted acordingly.
        /// </summary>
        /// <param name="euroAmount">Buy amount in euro requested by user</param>
        /// <param name="cryptoAmount">Calculated crypto amount</param>
        /// <param name="leverageRatio">Leverage multiplier selected by user</param>
        /// <param name="marginAmount">Calculated margin amount</param>
        /// <returns>Cardano market view</returns>
        [HttpPost]
        public IActionResult OpenCardanoPosition(string euroAmount, string cryptoAmount, string leverageRatio, string marginAmount)
        {
            var userId = GetUserDetails().Id;
            var currentBalances = walletProcedures.GetUsersWalletBalances(userId);
            var currentEUR = currentBalances.EuroAmount;
            var currentADA = currentBalances.CardanoAmount;

            marginAmount = marginAmount ?? "0";
            if (currentEUR < (decimal.Parse(euroAmount) + decimal.Parse(marginAmount)))
            {
                ViewBag.Error = "Insufficient funds for requested purchase!";
                return View("Cardano");
            }

            var newPosition = new PositionModel
            {
                DateTime = DateTime.Now,
                FiatAmount = decimal.Parse(euroAmount),
                CryptoAmount = decimal.Parse(cryptoAmount),
                Margin = decimal.Parse(marginAmount),
                Leverage = DbKeyConversionHelper.LeverageStringToDbKey(leverageRatio),
                Status = (int)StatusEnum.Open,
                Wallet = investmentProcedures.GetUserWalletId(userId, FiatEnum.EUR),
                Data = marketProcedures.GetLatestMarketData(CryptoEnum.ADA).Id
            };

            investmentProcedures.InsertNewPosition(newPosition);
            walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), currentEUR - newPosition.FiatAmount);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ADA.ToString(), currentADA + newPosition.CryptoAmount);

            return View("Cardano");
        }

        /// <summary>
        /// Receives user input form buy form. Validates amounts against users wallet.
        /// If requested purchase amount plus optional margin is larger than euro wallet balance,
        /// then further process is stopped and view bag with error message is prepared.
        /// If requested purchase amount plus optional margin is smaller or equal to euro wallet balance,
        /// then position model is filled, position inserted into DB and wallet balances adjusted acordingly.
        /// </summary>
        /// <param name="euroAmount">Buy amount in euro requested by user</param>
        /// <param name="cryptoAmount">Calculated crypto amount</param>
        /// <param name="leverageRatio">Leverage multiplier selected by user</param>
        /// <param name="marginAmount">Calculated margin amount</param>
        /// <returns>Cosmos market view</returns>
        [HttpPost]
        public IActionResult OpenCosmosPosition(string euroAmount, string cryptoAmount, string leverageRatio, string marginAmount)
        {
            var userId = GetUserDetails().Id;
            var currentBalances = walletProcedures.GetUsersWalletBalances(userId);
            var currentEUR = currentBalances.EuroAmount;
            var currentATOM = currentBalances.CosmosAmount;

            marginAmount = marginAmount ?? "0";
            if (currentEUR < (decimal.Parse(euroAmount) + decimal.Parse(marginAmount)))
            {
                ViewBag.Error = "Insufficient funds for requested purchase!";
                return View("Cosmos");
            }

            var newPosition = new PositionModel
            {
                DateTime = DateTime.Now,
                FiatAmount = decimal.Parse(euroAmount),
                CryptoAmount = decimal.Parse(cryptoAmount),
                Margin = decimal.Parse(marginAmount),
                Leverage = DbKeyConversionHelper.LeverageStringToDbKey(leverageRatio),
                Status = (int)StatusEnum.Open,
                Wallet = investmentProcedures.GetUserWalletId(userId, FiatEnum.EUR),
                Data = marketProcedures.GetLatestMarketData(CryptoEnum.ATOM).Id
            };

            investmentProcedures.InsertNewPosition(newPosition);
            walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), currentEUR - newPosition.FiatAmount);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.ATOM.ToString(), currentATOM + newPosition.CryptoAmount);

            return View("Cosmos");
        }

        /// <summary>
        /// Receives user input form buy form. Validates amounts against users wallet.
        /// If requested purchase amount plus optional margin is larger than euro wallet balance,
        /// then further process is stopped and view bag with error message is prepared.
        /// If requested purchase amount plus optional margin is smaller or equal to euro wallet balance,
        /// then position model is filled, position inserted into DB and wallet balances adjusted acordingly.
        /// </summary>
        /// <param name="euroAmount">Buy amount in euro requested by user</param>
        /// <param name="cryptoAmount">Calculated crypto amount</param>
        /// <param name="leverageRatio">Leverage multiplier selected by user</param>
        /// <param name="marginAmount">Calculated margin amount</param>
        /// <returns>Dogecoin market view</returns>
        [HttpPost]
        public IActionResult OpenDogecoinPosition(string euroAmount, string cryptoAmount, string leverageRatio, string marginAmount)
        {
            var userId = GetUserDetails().Id;
            var currentBalances = walletProcedures.GetUsersWalletBalances(userId);
            var currentEUR = currentBalances.EuroAmount;
            var currentDOGE = currentBalances.DogecoinAmount;

            marginAmount = marginAmount ?? "0";
            if (currentEUR < (decimal.Parse(euroAmount) + decimal.Parse(marginAmount)))
            {
                ViewBag.Error = "Insufficient funds for requested purchase!";
                return View("Dogecoin");
            }

            var newPosition = new PositionModel
            {
                DateTime = DateTime.Now,
                FiatAmount = decimal.Parse(euroAmount),
                CryptoAmount = decimal.Parse(cryptoAmount),
                Margin = decimal.Parse(marginAmount),
                Leverage = DbKeyConversionHelper.LeverageStringToDbKey(leverageRatio),
                Status = (int)StatusEnum.Open,
                Wallet = investmentProcedures.GetUserWalletId(userId, FiatEnum.EUR),
                Data = marketProcedures.GetLatestMarketData(CryptoEnum.DOGE).Id
            };

            investmentProcedures.InsertNewPosition(newPosition);
            walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), currentEUR - newPosition.FiatAmount);
            walletProcedures.UpdateUsersWalletBalance(userId, CryptoEnum.DOGE.ToString(), currentDOGE + newPosition.CryptoAmount);

            return View("Dogecoin");
        }

        [HttpPost]
        public IActionResult ClosePosition(string positionNumber, string cryptoSymbol)
        {
            var userId = GetUserDetails().Id;

            var positionsList = new List<PositionModel>();
            var returnablePage = string.Empty;

            if (cryptoSymbol == CryptoEnum.BTC.ToString())
            {
                positionsList = investmentProcedures.GetAllActivePositions(userId, CryptoEnum.BTC);
                returnablePage = "Bitcoin";
            }

            if (cryptoSymbol == CryptoEnum.ETH.ToString())
            {
                positionsList = investmentProcedures.GetAllActivePositions(userId, CryptoEnum.ETH);
                returnablePage = "Etherium";
            }

            if (cryptoSymbol == CryptoEnum.ADA.ToString())
            {
                positionsList = investmentProcedures.GetAllActivePositions(userId, CryptoEnum.ADA);
                returnablePage = "Cardano";
            }

            if (cryptoSymbol == CryptoEnum.ATOM.ToString())
            {
                positionsList = investmentProcedures.GetAllActivePositions(userId, CryptoEnum.ATOM);
                returnablePage = "Cosmos";
            }

            if (cryptoSymbol == CryptoEnum.DOGE.ToString())
            {
                positionsList = investmentProcedures.GetAllActivePositions(userId, CryptoEnum.DOGE);
                returnablePage = "Dogecoin";
            }

            var positionToClose = new PositionModel();
            var positionNumberInt = int.Parse(positionNumber);
            var counter = 1;

            foreach(var position in positionsList)
            {
                if(counter == positionNumberInt)
                {
                    positionToClose = position;
                }

                counter++;
            }

            var currentCryptoBalance = walletProcedures.GetSpecificWalletBalance(userId, cryptoSymbol);
            walletProcedures.UpdateUsersWalletBalance(userId, cryptoSymbol, (currentCryptoBalance - positionToClose.CryptoAmount));

            var buyTimeUnitValue = investmentProcedures.GetPositionsUnitValue(positionToClose.Id);
            var currentUnitValue = marketProcedures.GetLatestMarketData(StringToCryptoEnum(cryptoSymbol)).UnitValue;
            var profitMultiplier = currentUnitValue / buyTimeUnitValue;

            decimal newBalance;
            decimal currentFiatBalance;

            if (DbKeyConversionHelper.LeverageKeyToString(positionToClose.Leverage) == "2x")
            {
                currentFiatBalance = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());
                newBalance = currentFiatBalance + positionToClose.FiatAmount + ((positionToClose.FiatAmount * 2 * profitMultiplier) - (positionToClose.FiatAmount * 2));

                walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), newBalance);
            }
            else if (DbKeyConversionHelper.LeverageKeyToString(positionToClose.Leverage) == "5x")
            {
                currentFiatBalance = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());
                newBalance = currentFiatBalance + positionToClose.FiatAmount + ((positionToClose.FiatAmount * 5 * profitMultiplier) - (positionToClose.FiatAmount * 5));

                walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), newBalance);
            }
            else if (DbKeyConversionHelper.LeverageKeyToString(positionToClose.Leverage) == "10x")
            {
                currentFiatBalance = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());
                newBalance = currentFiatBalance + positionToClose.FiatAmount + ((positionToClose.FiatAmount * 10 * profitMultiplier) - (positionToClose.FiatAmount * 10));

                walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), newBalance);
            }
            else
            {
                currentFiatBalance = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());
                newBalance = currentFiatBalance + (positionToClose.FiatAmount * profitMultiplier);

                walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), newBalance);
            }

            investmentProcedures.UpdatePositionStatus(positionToClose.Id, (int)StatusEnum.Closed);

            return View(returnablePage);
        }

        /// <summary>
        /// Collects latest market data records from database for all supported cryptos.
        /// </summary>
        /// <returns>List of filled <see cref="MarketDataModel"/>s</returns>
        private static List<MarketDataModel> GetLatestMarketRecords()
        {
            var modelList = new List<MarketDataModel>
            {
                marketProcedures.GetLatestMarketData(CryptoEnum.BTC),
                marketProcedures.GetLatestMarketData(CryptoEnum.ETH),
                marketProcedures.GetLatestMarketData(CryptoEnum.ADA),
                marketProcedures.GetLatestMarketData(CryptoEnum.ATOM),
                marketProcedures.GetLatestMarketData(CryptoEnum.DOGE)
            };

            return modelList;
        }

        /// <summary>
        /// Makes a new market data request to CMC API for each supported crypto.
        /// Places collected data into <see cref="MarketDataModel"/>s.
        /// Makes a list of collected models.
        /// </summary>
        /// <returns>List of filled <see cref="MarketDataModel"/>s</returns>
        public List<MarketDataModel> GetNewMarketData()
        {
            var modelList = new List<MarketDataModel>();

            var btcFullData = GetCryptoToEuroData(CryptoEnum.BTC);
            var btcMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(btcFullData.Data.Bitcoin.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(btcFullData.Data.Bitcoin.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(btcFullData.Data.Bitcoin.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(btcMDM);

            var ethFullData = GetCryptoToEuroData(CryptoEnum.ETH);
            var ethMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(ethFullData.Data.Etherium.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(ethFullData.Data.Etherium.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(ethFullData.Data.Etherium.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ETH.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(ethMDM);

            var adaFullData = GetCryptoToEuroData(CryptoEnum.ADA);
            var adaMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(adaFullData.Data.Cardano.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(adaFullData.Data.Cardano.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(adaFullData.Data.Cardano.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ADA.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(adaMDM);

            var atomFullData = GetCryptoToEuroData(CryptoEnum.ATOM);
            var atomMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(atomFullData.Data.Cosmos.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(atomFullData.Data.Cosmos.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(atomFullData.Data.Cosmos.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ATOM.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(atomMDM);

            var dogeFullData = GetCryptoToEuroData(CryptoEnum.DOGE);
            var dogeMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(dogeFullData.Data.Dogecoin.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(dogeFullData.Data.Dogecoin.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(dogeFullData.Data.Dogecoin.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.DOGE.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(dogeMDM);

            return modelList;
        }

        /// <summary>
        /// Makes a Coin Market Cap API request for specified cryptocurrency.
        /// Executes request and deserializes response into request models.
        /// </summary>
        /// <param name="crypto">Data will be collected for this crypto.</param>
        /// <returns>Filled <see cref="Root"/> response model</returns>
        /// <exception cref="Exception"></exception>
        private static Root GetCryptoToEuroData(CryptoEnum crypto)
        {
            var request = new RestRequest(CoinMarketCapApiConstants.LatestQuotesTest, Method.Get);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("symbol", crypto.ToString());
            request.AddParameter("convert", FiatEnum.EUR.ToString());
            request.AddParameter("CMC_PRO_API_KEY", CoinMarketCapApiConstants.AccessKey);

            var response = new RestClient().Execute(request);

            if (!response.IsSuccessStatusCode || response.Content == null)
            {
                throw new Exception("Market data request has failed!");
            }

            var responseModel = JsonConvert.DeserializeObject<Root>(response.Content);

            if (responseModel == null)
            {
                throw new Exception("Market data deserialization has failed!");
            }

            return responseModel;
        }

        /// <summary>
        /// Iterates trough each model in received model list 
        /// and calls insert procedure for each model to insert data into database.
        /// </summary>
        /// <param name="modelList">List of filled <see cref="MarketDataModel"/>s</param>
        public void InsertMarketData(List<MarketDataModel> modelList)
        {
            foreach (var model in modelList)
            {
                marketProcedures.InsertNewMarketDataEntry(model);
            }
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

        private CryptoEnum StringToCryptoEnum(string symbol)
        {
            return symbol switch
            {
                "BTC" => CryptoEnum.BTC,
                "ETH" => CryptoEnum.ETH,
                "ADA" => CryptoEnum.ADA,
                "ATOM" => CryptoEnum.ATOM,
                "DOGE" => CryptoEnum.DOGE,
                _ => throw new ArgumentException(symbol)
            };
        }
    }
}
