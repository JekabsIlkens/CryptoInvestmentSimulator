using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Enums;
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
                throw new ArgumentNullException(nameof(marketDataModel));
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

        /// <summary>
        /// Collects the latest record of data for specified cryptocurrency.
        /// </summary>
        /// <param name="crypto">Currency to collect data for</param>
        /// <returns>Filled <see cref="MarketDataModel"/></returns>
        public MarketDataModel GetLatestMarketData(CryptoEnum crypto)
        {
            MarketDataModel marketDataModel = new MarketDataModel();

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new(
                    $"SELECT * FROM market_data WHERE crypto_symbol = '{crypto}' ORDER BY data_id DESC LIMIT 1",
                    connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        marketDataModel.CryptoSymbol = reader.GetValue(reader.GetOrdinal("crypto_symbol")).ToString();
                        marketDataModel.FiatSymbol = reader.GetValue(reader.GetOrdinal("fiat_symbol")).ToString();

                        var unitValue = reader.GetValue(reader.GetOrdinal("unit_value")).ToString();
                        marketDataModel.FiatPricePerUnit = decimal.Parse(unitValue);

                        var dateTime = reader.GetValue(reader.GetOrdinal("date_time")).ToString();
                        marketDataModel.CollectionDateTime = DateTime.Parse(dateTime);

                        var dailyChange = reader.GetValue(reader.GetOrdinal("daily_change")).ToString();
                        marketDataModel.PercentChange24h = decimal.Parse(dailyChange);

                        var weeklyChange = reader.GetValue(reader.GetOrdinal("weekly_change")).ToString();
                        marketDataModel.PercentChange7d = decimal.Parse(weeklyChange);
                    }
                }
            }

            return marketDataModel;
        }

        /// <summary>
        /// Collects specified amount of historical price points
        /// into an array for specified cryptocurrency.
        /// </summary>
        /// <param name="crypto">Which crpto data</param>
        /// <param name="rowCount">How many rows to return</param>
        /// <param name="everyNth">Will use evert nth row</param>
        /// <returns>Array of price points of type double</returns>
        /// <exception cref="ArgumentException"></exception>
        public double[] GetPricePointHistory(CryptoEnum crypto, int rowCount, int everyNth)
        {
            if (rowCount <= 0)
            {
                throw new ArgumentException($"Requested {rowCount} rows! Invalid!");
            }

            double[] pricePoints = new double[rowCount];

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new(
                    $"SELECT unit_value FROM " +
                    $"(SELECT * FROM market_data WHERE crypto_symbol = '{crypto}' AND data_id mod {everyNth} = 0 ORDER BY data_id DESC LIMIT {rowCount}) AS sub " +
                    $"ORDER BY data_id ASC ", 
                    connection);

                using (var reader = command.ExecuteReader())
                {
                    var num = 0;
                    while (reader.Read())
                    {
                        var pricePointStr = reader.GetValue(reader.GetOrdinal("unit_value")).ToString();
                        var pricePoint = double.Parse(pricePointStr);

                        pricePoints[num] = pricePoint;
                        num++;
                    }
                }
            }

            return pricePoints;
        }

        /// <summary>
        /// Collects specified amount of historical date/time points
        /// into an array for specified cryptocurrency.
        /// </summary>
        /// <param name="crypto">Which crpto data</param>
        /// <param name="rowCount">How many rows to return</param>
        /// <param name="everyNth">Will use evert nth row</param>
        /// <returns>Array of date/time points of type long</returns>
        /// <exception cref="ArgumentException"></exception>
        public long[] GetTimePointHistory(CryptoEnum crypto, int rowCount, int everyNth)
        {
            if (rowCount <= 0)
            {
                throw new ArgumentException($"Requested {rowCount} rows! Invalid!");
            }

            long[] timePoints = new long[rowCount];

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new(
                    $"SELECT date_time FROM " +
                    $"(SELECT * FROM market_data WHERE crypto_symbol = '{crypto}' AND data_id mod {everyNth} = 0 ORDER BY data_id DESC LIMIT {rowCount}) AS sub " +
                    $"ORDER BY data_id ASC ",
                    connection);

                using (var reader = command.ExecuteReader())
                {
                    var num = 0;
                    while (reader.Read())
                    {
                        var timePointStr = reader.GetValue(reader.GetOrdinal("date_time")).ToString();
                        var timePoint = ((DateTimeOffset)DateTime.Parse(timePointStr)).ToUnixTimeSeconds() * 1000;

                        timePoints[num] = timePoint;
                        num++;
                    }
                }
            }

            return timePoints;
        }
    }
}
