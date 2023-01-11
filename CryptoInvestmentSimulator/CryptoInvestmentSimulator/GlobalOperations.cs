using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Helpers;
using CryptoInvestmentSimulator.Models.ResponseModels;
using CryptoInvestmentSimulator.Models.ViewModels;
using Newtonsoft.Json;
using RestSharp;

namespace CryptoInvestmentSimulator
{
    public class GlobalOperations
    {
        private static readonly DatabaseContext context = new(DatabaseConstants.ProductionAccess);
        private static readonly UserProcedures userProcedures = new(context);
        private static readonly WalletProcedures walletProcedures = new(context);
        private static readonly MarketDataProcedures marketProcedures = new(context);
        private static readonly InvestmentProcedures investmentProcedures = new(context);
        public static Random valueMocker = new();

        /// <summary>
        /// Gets all open leveraged positions for each verified user.
        /// Calculates current potential profits or losses for each position.
        /// If losses exceed margin amount 4 times - liquidates position and adjusts users wallet balances.
        /// </summary>
        public void LiquidateBadPositions()
        {
            // Collects all users that are able to access the market.
            var userIdList = userProcedures.GetAllVerifiedUserIds();

            // Collects a list of all open leveraged positions for each user
            foreach (var userId in userIdList)
            {
                var activeLeveragedPositions = investmentProcedures.GetAllOpenLeveragedPositions(userId);

                // Begins liquidation check for each position in collected positions list.
                foreach (var position in activeLeveragedPositions)
                {
                    var positionCryptoEnum = InternalConversionHelper.IntToCryptoEnum(position.CryptoId);

                    // Gets the latest unit value for specific asset and calculates value used to determine profits.
                    var currentUnitValue = marketProcedures.GetLatestMarketData(positionCryptoEnum).UnitValue;
                    var profitMultiplier = currentUnitValue / position.UnitValue;

                    decimal currentProfit;

                    // Filters which leverage the given position uses and calculates the current profit or loss.
                    if (position.RatioId == (int)LeverageEnum.Two)
                    {
                        currentProfit = ((position.FiatAmount * 2 * profitMultiplier) - (position.FiatAmount * 2));
                    }
                    else if (position.RatioId == (int)LeverageEnum.Five)
                    {
                        currentProfit = ((position.FiatAmount * 5 * profitMultiplier) - (position.FiatAmount * 5));
                    }
                    else
                    {
                        currentProfit = ((position.FiatAmount * 10 * profitMultiplier) - (position.FiatAmount * 10));
                    }

                    // If the position is more than 4 times the margin amount begin liquidation process.
                    if (currentProfit < (position.MarginAmount * 2 * -1))
                    {
                        // Removes positions crypto amount from users crypto wallet.
                        var cryptoBalance = walletProcedures.GetSpecificWalletBalance(userId, positionCryptoEnum.ToString());
                        walletProcedures.UpdateUsersWalletBalance(userId, positionCryptoEnum.ToString(), (cryptoBalance - position.CryptoAmount));

                        // Returns leftovers of euro amount to users fiat wallet.
                        // Initial investment amount minus losses, without returning the margin.
                        var fiatBalance = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());
                        walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), (fiatBalance + position.FiatAmount - currentProfit));

                        // Updates positions status to Liquidated.
                        investmentProcedures.UpdatePositionStatus(position.positionId, (int)StatusEnum.Liquidated);
                    }
                }
            }
        }

        /// <summary>
        /// Gets insertion ready market data model list and calls insert procedure
        /// for each model to insert data into database.
        /// </summary>
        public void CollectLatestMarketData()
        {
            var modelList = GetInsertionReadyMarketData();

            foreach (var model in modelList)
            {
                marketProcedures.InsertNewMarketDataEntry(model);
            }
        }

        /// <summary>
        /// Makes a new latest market data request to Coin Market Cap API for each supported cryptocurrency.
        /// Processes collected data and fills a list of <see cref="MarketDataModel"/>s.
        /// </summary>
        /// <returns>
        /// List of filled <see cref="MarketDataModel"/>s
        /// </returns>
        private static List<MarketDataModel> GetInsertionReadyMarketData()
        {
            var modelList = new List<MarketDataModel>();

            // Collects raw request data as it's named and structured in json response.
            // Collects only data necessary for market_data table. Formats unit value to six floating points and growth to percent.
            var btcRawData = GetCryptoToFiatLatestQuote(CryptoEnum.BTC, FiatEnum.EUR);           
            var btcMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToSix(btcRawData.Data.Bitcoin.Quote.Euro.Price * valueMocker.Next(15000, 16001)),
                Change24h = FloatingPointHelper.FloatingPointToTwo(btcRawData.Data.Bitcoin.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(btcRawData.Data.Bitcoin.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(btcMDM);

            // Collects raw request data as it's named and structured in json response.
            // Collects only data necessary for market_data table. Formats unit value to six floating points and growth to percent.
            var ethRawData = GetCryptoToFiatLatestQuote(CryptoEnum.ETH, FiatEnum.EUR);
            var ethMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToSix(ethRawData.Data.Etherium.Quote.Euro.Price * valueMocker.Next(1000, 1501)),
                Change24h = FloatingPointHelper.FloatingPointToTwo(ethRawData.Data.Etherium.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(ethRawData.Data.Etherium.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ETH.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(ethMDM);

            // Collects raw request data as it's named and structured in json response.
            // Collects only data necessary for market_data table. Formats unit value to six floating points and growth to percent.
            var adaRawData = GetCryptoToFiatLatestQuote(CryptoEnum.ADA, FiatEnum.EUR);
            var adaMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToSix(adaRawData.Data.Cardano.Quote.Euro.Price * valueMocker.Next(1, 4)),
                Change24h = FloatingPointHelper.FloatingPointToTwo(adaRawData.Data.Cardano.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(adaRawData.Data.Cardano.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ADA.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(adaMDM);

            // Collects raw request data as it's named and structured in json response.
            // Collects only data necessary for market_data table. Formats unit value to six floating points and growth to percent.
            var atomRawData = GetCryptoToFiatLatestQuote(CryptoEnum.ATOM, FiatEnum.EUR);
            var atomMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToSix(atomRawData.Data.Cosmos.Quote.Euro.Price * valueMocker.Next(10, 16)),
                Change24h = FloatingPointHelper.FloatingPointToTwo(atomRawData.Data.Cosmos.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(atomRawData.Data.Cosmos.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ATOM.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(atomMDM);

            // Collects raw request data as it's named and structured in json response.
            // Collects only data necessary for market_data table. Formats unit value to six floating points and growth to percent.
            var dogeRawData = GetCryptoToFiatLatestQuote(CryptoEnum.DOGE, FiatEnum.EUR);
            var dogeMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToSix(dogeRawData.Data.Dogecoin.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(dogeRawData.Data.Dogecoin.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(dogeRawData.Data.Dogecoin.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.DOGE.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(dogeMDM);

            return modelList;
        }

        /// <summary>
        /// Makes a Coin Market Cap API request for specified cryptocurrency.
        /// Executes request and deserializes response into <see cref="Root"/> model.
        /// </summary>
        /// <param name="crypto">Cryptocurrency to collect data for.</param>
        /// <returns>
        /// Filled <see cref="Root"/> response model.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when request or deserialization fails.
        /// </exception>
        private static Root GetCryptoToFiatLatestQuote(CryptoEnum crypto, FiatEnum fiat)
        {
            var request = new RestRequest(CoinMarketCapApiConstants.LatestQuotesTest, Method.Get);

            // Prepares request header and parameters with necessary values.
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("symbol", crypto.ToString());
            request.AddParameter("convert", fiat.ToString());
            request.AddParameter("CMC_PRO_API_KEY", CoinMarketCapApiConstants.AccessKey);

            // Executes API call and collects response.
            var response = new RestClient().Execute(request);

            if (!response.IsSuccessStatusCode || response.Content == null)
            {
                throw new Exception("Market data request has failed!");
            }

            // Deserializes json object into pre-prepared request models.
            var responseModel = JsonConvert.DeserializeObject<Root>(response.Content);

            if (responseModel == null)
            {
                throw new Exception("Market data deserialization has failed!");
            }

            return responseModel;
        }
    }
}
