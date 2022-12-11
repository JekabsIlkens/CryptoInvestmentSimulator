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

namespace CryptoInvestmentSimulator.Controllers
{
    public class MarketController : Controller
    {
        private static readonly DatabaseContext context = new(DatabaseConstants.Access);
        private static readonly MarketDataProcedures procedures = new(context);

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
        /// Collects Bitcoin market data history for given cryptocurrency
        /// and prepares view bags for use in dynamic chart generation.
        /// </summary>
        /// <returns>Partial view that renders 1h chart</returns>
        [Authorize]
        public IActionResult BTC1hChart()
        {
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.BTC, 60, 1);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.BTC, 60, 1);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.BTC, 240, 4);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.BTC, 240, 4);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.BTC, 480, 8);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.BTC, 480, 8);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.BTC, 1440, 24);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.BTC, 1440, 24);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ETH, 60, 1);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ETH, 60, 1);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ETH, 240, 4);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ETH, 240, 4);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ETH, 480, 8);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ETH, 480, 8);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ETH, 1440, 24);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ETH, 1440, 24);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ADA, 60, 1);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ADA, 60, 1);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ADA, 240, 4);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ADA, 240, 4);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ADA, 480, 8);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ADA, 480, 8);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ADA, 1440, 24);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ADA, 1440, 24);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ATOM, 60, 1);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ATOM, 60, 1);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ATOM, 240, 4);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ATOM, 240, 4);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ATOM, 480, 8);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ATOM, 480, 8);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.ATOM, 1440, 24);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.ATOM, 1440, 24);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.DOGE, 60, 1);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.DOGE, 60, 1);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.DOGE, 240, 4);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.DOGE, 240, 4);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.DOGE, 480, 8);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.DOGE, 480, 8);

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
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.DOGE, 1440, 24);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.DOGE, 1440, 24);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart24h");
        }

        /// <summary>
        /// Collects latest market data records from database for all supported cryptos.
        /// </summary>
        /// <returns>List of filled <see cref="MarketDataModel"/>s</returns>
        public List<MarketDataModel> GetLatestMarketRecords()
        {
            var modelList = new List<MarketDataModel>
            {
                procedures.GetLatestMarketData(CryptoEnum.BTC),
                procedures.GetLatestMarketData(CryptoEnum.ETH),
                procedures.GetLatestMarketData(CryptoEnum.ADA),
                procedures.GetLatestMarketData(CryptoEnum.ATOM),
                procedures.GetLatestMarketData(CryptoEnum.DOGE)
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
                CryptoSymbol = btcFullData.Data.Bitcoin.Symbol,
                FiatSymbol = FiatEnum.EUR.ToString(),
                CollectionDateTime = DateTime.Now,
                FiatPricePerUnit = FloatingPointHelper.FloatingPointToFour(btcFullData.Data.Bitcoin.Quote.Euro.Price),
                PercentChange24h = FloatingPointHelper.FloatingPointToTwo(btcFullData.Data.Bitcoin.Quote.Euro.PercentChange24h) * 100,
                PercentChange7d = FloatingPointHelper.FloatingPointToTwo(btcFullData.Data.Bitcoin.Quote.Euro.PercentChange7d) * 100,
            };
            modelList.Add(btcMDM);

            var ethFullData = GetCryptoToEuroData(CryptoEnum.ETH);
            var ethMDM = new MarketDataModel()
            {
                CryptoSymbol = ethFullData.Data.Etherium.Symbol,
                FiatSymbol = FiatEnum.EUR.ToString(),
                CollectionDateTime = DateTime.Now,
                FiatPricePerUnit = FloatingPointHelper.FloatingPointToFour(ethFullData.Data.Etherium.Quote.Euro.Price),
                PercentChange24h = FloatingPointHelper.FloatingPointToTwo(ethFullData.Data.Etherium.Quote.Euro.PercentChange24h) * 100,
                PercentChange7d = FloatingPointHelper.FloatingPointToTwo(ethFullData.Data.Etherium.Quote.Euro.PercentChange7d) * 100,
            };
            modelList.Add(ethMDM);

            var atomFullData = GetCryptoToEuroData(CryptoEnum.ATOM);
            var atomMDM = new MarketDataModel()
            {
                CryptoSymbol = atomFullData.Data.Cosmos.Symbol,
                FiatSymbol = FiatEnum.EUR.ToString(),
                CollectionDateTime = DateTime.Now,
                FiatPricePerUnit = FloatingPointHelper.FloatingPointToFour(atomFullData.Data.Cosmos.Quote.Euro.Price),
                PercentChange24h = FloatingPointHelper.FloatingPointToTwo(atomFullData.Data.Cosmos.Quote.Euro.PercentChange24h) * 100,
                PercentChange7d = FloatingPointHelper.FloatingPointToTwo(atomFullData.Data.Cosmos.Quote.Euro.PercentChange7d) * 100,
            };
            modelList.Add(atomMDM);

            var adaFullData = GetCryptoToEuroData(CryptoEnum.ADA);
            var adaMDM = new MarketDataModel()
            {
                CryptoSymbol = adaFullData.Data.Cardano.Symbol,
                FiatSymbol = FiatEnum.EUR.ToString(),
                CollectionDateTime = DateTime.Now,
                FiatPricePerUnit = FloatingPointHelper.FloatingPointToFour(adaFullData.Data.Cardano.Quote.Euro.Price),
                PercentChange24h = FloatingPointHelper.FloatingPointToTwo(adaFullData.Data.Cardano.Quote.Euro.PercentChange24h) * 100,
                PercentChange7d = FloatingPointHelper.FloatingPointToTwo(adaFullData.Data.Cardano.Quote.Euro.PercentChange7d) * 100,
            };
            modelList.Add(adaMDM);

            var dogeFullData = GetCryptoToEuroData(CryptoEnum.DOGE);
            var dogeMDM = new MarketDataModel()
            {
                CryptoSymbol = dogeFullData.Data.Dogecoin.Symbol,
                FiatSymbol = FiatEnum.EUR.ToString(),
                CollectionDateTime = DateTime.Now,
                FiatPricePerUnit = FloatingPointHelper.FloatingPointToFour(dogeFullData.Data.Dogecoin.Quote.Euro.Price),
                PercentChange24h = FloatingPointHelper.FloatingPointToTwo(dogeFullData.Data.Dogecoin.Quote.Euro.PercentChange24h) * 100,
                PercentChange7d = FloatingPointHelper.FloatingPointToTwo(dogeFullData.Data.Dogecoin.Quote.Euro.PercentChange7d) * 100,
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
                procedures.InsertNewMarketDataEntry(model);
            }
        }
    }
}
