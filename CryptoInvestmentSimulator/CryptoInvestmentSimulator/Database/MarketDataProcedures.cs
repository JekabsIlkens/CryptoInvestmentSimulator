using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Helpers;
using CryptoInvestmentSimulator.Models.ViewModels;
using MySql.Data.MySqlClient;

namespace CryptoInvestmentSimulator.Database
{
    public class MarketDataProcedures
    {
        private readonly DatabaseContext context;

        public MarketDataProcedures(DatabaseContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Inserts new market data into database.
        /// </summary>
        /// <param name="marketDataModel"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void InsertNewMarketDataEntry(MarketDataModel marketDataModel)
        {
            if (marketDataModel == null)
            {
                throw new ArgumentNullException($"Model {nameof(marketDataModel)} is null or empty!");
            }

            var formattedDateTime = DateTimeFormatHelper.ToDbFormatAsString(marketDataModel.CollectionDateTime);

            var valuesString = $"'{marketDataModel.CryptoSymbol}', '{marketDataModel.FiatSymbol}', '{formattedDateTime}'," +
                $" {marketDataModel.FiatPricePerUnit}, {marketDataModel.PercentChange24h}, {marketDataModel.PercentChange7d}";

            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({valuesString})", connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
