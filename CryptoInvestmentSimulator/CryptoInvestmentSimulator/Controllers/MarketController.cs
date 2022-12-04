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
            var marketData = GetMarketData();
            return View(marketData);
        }

        public IActionResult Chart()
        {
            var pricePoints = procedures.GetPricePointHistory(CryptoEnum.BTC, 80);
            var timePoints = procedures.GetTimePointHistory(CryptoEnum.BTC, 80);

            ViewBag.PricePoints = pricePoints;
            ViewBag.TimePoints = timePoints;

            return PartialView("_Chart");
        }

        [Authorize]
        public IActionResult Bitcoin()
        {
            SetupMarketHistory(CryptoEnum.BTC);
            return View();
        }

        [Authorize]
        public IActionResult Etherium()
        {
            SetupMarketHistory(CryptoEnum.ETH);
            return View();
        }

        [Authorize]
        public IActionResult Cardano()
        {
            SetupMarketHistory(CryptoEnum.ADA);
            return View();
        }

        [Authorize]
        public IActionResult Cosmos()
        {
            SetupMarketHistory(CryptoEnum.ATOM);
            return View();
        }

        [Authorize]
        public IActionResult Dogecoin()
        {
            SetupMarketHistory(CryptoEnum.DOGE);
            return View();
        }

        /// <summary>
        /// Retrieves specified amount of historical chart points for specified cryptocurrency.
        /// Sets chart update interval and places everything in view bag for chart rendering.
        /// </summary>
        /// <param name="crypto"></param>
        public void SetupMarketHistory(CryptoEnum crypto)
        {
            var chartPoints = 0;
            var pricePoint = 0;
            var timePoint = 0;

            int updateInterval = 2000;

            ViewBag.ChartPoints = JsonConvert.SerializeObject(chartPoints);
            ViewBag.PricePoint = pricePoint;
            ViewBag.TimePoint = timePoint;
            ViewBag.UpdateInterval = updateInterval;
        }

        /// <summary>
        /// Gets market data for all supported cryptos and creates view models for them.
        /// </summary>
        /// <returns>List of models</returns>
        public List<MarketDataModel> GetMarketData()
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

        public void InsertMarketData(List<MarketDataModel> modelList)
        {
            var context = new DatabaseContext(DatabaseConstants.Access);
            var procedure = new MarketDataProcedures(context);

            foreach (var model in modelList)
            {
                procedure.InsertNewMarketDataEntry(model);
            }
        }
    }
}
