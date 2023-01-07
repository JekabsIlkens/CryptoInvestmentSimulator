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
        private static readonly DatabaseContext context = new(DatabaseConstants.Access);
        private static readonly UserProcedures userProcedures = new(context);
        private static readonly WalletProcedures walletProcedures = new(context);
        private static readonly MarketDataProcedures marketProcedures = new(context);
        private static readonly InvestmentProcedures investmentProcedures = new(context);

        /// <summary>
        /// Gets all open leveraged positions for each verified user.
        /// Calculates current potential profits or losses for each position.
        /// If losses are double the margin value - liquidates position and adjusts users wallet balances.
        /// </summary>
        public void LiquidateBadPositions()
        {
            var userIdList = userProcedures.GetAllVerifiedUserIds();

            foreach (var userId in userIdList)
            {
                var activeLeveragedPositions = investmentProcedures.GetAllOpenLeveragedPositions(userId);

                foreach (var position in activeLeveragedPositions)
                {
                    var positionCryptoEnum = InternalConversionHelper.IntToCryptoEnum(position.CryptoId);

                    var currentUnitValue = marketProcedures.GetLatestMarketData(positionCryptoEnum).UnitValue;
                    var profitMultiplier = currentUnitValue / position.UnitValue;

                    decimal currentProfit;

                    if (position.RatioId == 2)
                    {
                        currentProfit = ((position.FiatAmount * 2 * profitMultiplier) - (position.FiatAmount * 2));
                    }
                    else if (position.RatioId == 3)
                    {
                        currentProfit = ((position.FiatAmount * 5 * profitMultiplier) - (position.FiatAmount * 5));
                    }
                    else
                    {
                        currentProfit = ((position.FiatAmount * 10 * profitMultiplier) - (position.FiatAmount * 10));
                    }

                    if (currentProfit < (position.MarginAmount * 2 * -1))
                    {
                        var cryptoBalance = walletProcedures.GetSpecificWalletBalance(userId, positionCryptoEnum.ToString());
                        walletProcedures.UpdateUsersWalletBalance(userId, positionCryptoEnum.ToString(), (cryptoBalance - position.CryptoAmount));

                        var fiatBalance = walletProcedures.GetSpecificWalletBalance(userId, FiatEnum.EUR.ToString());
                        walletProcedures.UpdateUsersWalletBalance(userId, FiatEnum.EUR.ToString(), (fiatBalance + position.FiatAmount - currentProfit));

                        investmentProcedures.UpdatePositionStatus(position.TransactionId, (int)StatusEnum.Liquidated);
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
        private List<MarketDataModel> GetInsertionReadyMarketData()
        {
            var modelList = new List<MarketDataModel>();

            var btcRawData = GetCryptoToFiatLatestQuote(CryptoEnum.BTC, FiatEnum.EUR);
            var btcMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(btcRawData.Data.Bitcoin.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(btcRawData.Data.Bitcoin.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(btcRawData.Data.Bitcoin.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.BTC.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(btcMDM);

            var ethRawData = GetCryptoToFiatLatestQuote(CryptoEnum.ETH, FiatEnum.EUR);
            var ethMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(ethRawData.Data.Etherium.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(ethRawData.Data.Etherium.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(ethRawData.Data.Etherium.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ETH.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(ethMDM);

            var adaRawData = GetCryptoToFiatLatestQuote(CryptoEnum.ADA, FiatEnum.EUR);
            var adaMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(adaRawData.Data.Cardano.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(adaRawData.Data.Cardano.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(adaRawData.Data.Cardano.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ADA.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(adaMDM);

            var atomRawData = GetCryptoToFiatLatestQuote(CryptoEnum.ATOM, FiatEnum.EUR);
            var atomMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(atomRawData.Data.Cosmos.Quote.Euro.Price),
                Change24h = FloatingPointHelper.FloatingPointToTwo(atomRawData.Data.Cosmos.Quote.Euro.PercentChange24h) * 100,
                Change7d = FloatingPointHelper.FloatingPointToTwo(atomRawData.Data.Cosmos.Quote.Euro.PercentChange7d) * 100,
                CryptoSymbol = CryptoEnum.ATOM.ToString(),
                FiatSymbol = FiatEnum.EUR.ToString(),
            };
            modelList.Add(atomMDM);

            var dogeRawData = GetCryptoToFiatLatestQuote(CryptoEnum.DOGE, FiatEnum.EUR);
            var dogeMDM = new MarketDataModel()
            {
                CollectionTime = DateTime.Now,
                UnitValue = FloatingPointHelper.FloatingPointToFour(dogeRawData.Data.Dogecoin.Quote.Euro.Price),
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

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("symbol", crypto.ToString());
            request.AddParameter("convert", fiat.ToString());
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
    }
}
