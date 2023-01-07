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

        public void LiquidatePositions()
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
        /// Iterates trough each model in received model list 
        /// and calls insert procedure for each model to insert data into database.
        /// </summary>
        public void InsertMarketData()
        {
            var modelList = GetNewMarketData();

            foreach (var model in modelList)
            {
                marketProcedures.InsertNewMarketDataEntry(model);
            }
        }

        /// <summary>
        /// Makes a new market data request to CMC API for each supported crypto.
        /// Places collected data into <see cref="MarketDataModel"/>s.
        /// Makes a list of collected models.
        /// </summary>
        /// <returns>List of filled <see cref="MarketDataModel"/>s</returns>
        private List<MarketDataModel> GetNewMarketData()
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
    }
}
