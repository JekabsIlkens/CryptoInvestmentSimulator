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
        /// Inserts received <see cref="MarketDataModel"/> into database.
        /// </summary>
        /// <param name="marketDataModel">Filled <see cref="MarketDataModel"/>.</param>
        public void InsertNewMarketDataEntry(MarketDataModel marketDataModel)
        {
            var formattedDateTime = DateTimeFormatHelper.ToDbFormatAsString(marketDataModel.CollectionTime);
            var cryptoKey = DatabaseKeyConversionHelper.CryptoSymbolToDbKey(marketDataModel.CryptoSymbol);
            var fiatKey = DatabaseKeyConversionHelper.FiatSymbolToDbKey(marketDataModel.FiatSymbol);

            var valuesString = $"'{formattedDateTime}', {marketDataModel.UnitValue}, {marketDataModel.Change24h}, " +
                $"{marketDataModel.Change7d}, {cryptoKey}, {fiatKey}";

            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({valuesString})", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Collects the latest data record for specified cryptocurrency.
        /// </summary>
        /// <param name="crypto">Currency to collect data for.</param>
        /// <returns>
        /// Filled <see cref="MarketDataModel"/>.
        /// </returns>
        public MarketDataModel GetLatestMarketData(CryptoEnum crypto)
        {
            MarketDataModel marketDataModel = new MarketDataModel();

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new(
                    $"SELECT * FROM market_data WHERE crypto_id = {(int)crypto} ORDER BY data_id DESC LIMIT 1",
                    connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dataId = (int)reader.GetValue(reader.GetOrdinal("data_id"));
                        marketDataModel.Id = dataId;

                        var dateTime = reader.GetValue(reader.GetOrdinal("date_time")).ToString();
                        marketDataModel.CollectionTime = DateTime.Parse(dateTime);

                        var unitValue = reader.GetValue(reader.GetOrdinal("unit_value")).ToString();
                        marketDataModel.UnitValue = decimal.Parse(unitValue);

                        var dailyChange = reader.GetValue(reader.GetOrdinal("daily_change")).ToString();
                        marketDataModel.Change24h = decimal.Parse(dailyChange);

                        var weeklyChange = reader.GetValue(reader.GetOrdinal("weekly_change")).ToString();
                        marketDataModel.Change7d = decimal.Parse(weeklyChange);

                        var cryptoSymbol = DatabaseKeyConversionHelper.CryptoKeyToSymbol((int)reader.GetValue(reader.GetOrdinal("crypto_id")));
                        var fiatSymbol = DatabaseKeyConversionHelper.FiatKeyToSymbol((int)reader.GetValue(reader.GetOrdinal("fiat_id")));

                        marketDataModel.CryptoSymbol = cryptoSymbol;
                        marketDataModel.FiatSymbol = fiatSymbol;
                    }
                }
            }

            return marketDataModel;
        }

        /// <summary>
        /// Collects specified amount of historical price points
        /// into an array for specified cryptocurrency.
        /// </summary>
        /// <param name="crypto">Which crpto data.</param>
        /// <param name="rowCount">How many rows to return.</param>
        /// <param name="everyNth">Will use evert nth row.</param>
        /// <returns>
        /// Double array of price points.
        /// </returns>
        public double[] GetPricePointHistory(CryptoEnum crypto, int rowCount, int everyNth)
        {
            double[] pricePoints = new double[rowCount];

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new(
                    $"SELECT unit_value FROM " +
                    $"(SELECT * FROM market_data WHERE crypto_id = {(int)crypto} AND data_id mod {everyNth} = 0 ORDER BY data_id DESC LIMIT {rowCount}) AS sub " +
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
        /// <param name="crypto">Which crpto data.</param>
        /// <param name="rowCount">How many rows to return.</param>
        /// <param name="everyNth">Will use evert nth row.</param>
        /// <returns>
        /// Array of date/time points of type long.
        /// </returns>
        public long[] GetTimePointHistory(CryptoEnum crypto, int rowCount, int everyNth)
        {
            long[] timePoints = new long[rowCount];

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new(
                    $"SELECT date_time FROM " +
                    $"(SELECT * FROM market_data WHERE crypto_id = {(int)crypto} AND data_id mod {everyNth} = 0 ORDER BY data_id DESC LIMIT {rowCount}) AS sub " +
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
