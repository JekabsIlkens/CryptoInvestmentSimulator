using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Helpers;
using MySql.Data.MySqlClient;
using MySql.Server;

namespace UnitTests.Mocks
{
    public class DatabaseMock
    {
        /// <summary>
        /// Creates a mock database for testing purpouses.
        /// Tables matching real database are created and populated with
        /// data for testing specific scenarios.
        /// </summary>
        /// <returns>Configured <see cref="MySqlServer.Instance"/></returns>
        public static MySqlServer CreateDatabase()
        {
            // Sets up a new mock sql server instance.
            MySqlServer dbServer = MySqlServer.Instance;

            // Starts the mock server.
            dbServer.StartServer();

            // Creates a mock database.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString(), "CREATE DATABASE test;");

            // Creates a mock time zone table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `time_zone` (" +
                "`zone_id` int NOT NULL AUTO_INCREMENT, " +
                "`change` int NOT NULL, " +
                "PRIMARY KEY (`zone_id`))");

            // Creates a mock user table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `user` (" +
                "`user_id` int NOT NULL AUTO_INCREMENT, " +
                "`email` varchar(45) NOT NULL, " +
                "`verified` int NOT NULL, " +
                "`username` varchar(16) NOT NULL, " +
                "`avatar` varchar(512) NOT NULL, " +
                "`zone_id` int NOT NULL, " +
                "PRIMARY KEY (`user_id`), " +
                "KEY `fk_user_time_zone_idx` (`zone_id`), " +
                "CONSTRAINT `fk_user_time_zone` FOREIGN KEY (`zone_id`) REFERENCES `time_zone` (`zone_id`))");

            // Creates a mock crypto symbol table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `crypto_symbol` (" +
                "`crypto_id` int NOT NULL AUTO_INCREMENT, " +
                "`symbol` varchar(4) NOT NULL, " +
                "PRIMARY KEY (`crypto_id`))");

            // Creates a mock fiat symbol table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `fiat_symbol` (" +
                "`fiat_id` int NOT NULL AUTO_INCREMENT, " +
                "`symbol` varchar(4) NOT NULL, " +
                "PRIMARY KEY (`fiat_id`))");

            // Creates a mock market data table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `market_data` (" +
                "`data_id` int NOT NULL AUTO_INCREMENT, " +
                "`date_time` datetime NOT NULL, " +
                "`unit_value` decimal(12,6) NOT NULL, " +
                "`daily_change` decimal(6,2) NOT NULL, " +
                "`weekly_change` decimal(6,2) NOT NULL, " +
                "`crypto_id` int NOT NULL, " +
                "`fiat_id` int NOT NULL, " +
                "PRIMARY KEY (`data_id`), " +
                "KEY `fk_market_data_crypto_symbol1_idx` (`crypto_id`), " +
                "KEY `fk_market_data_fiat_symbol1_idx` (`fiat_id`), " +
                "CONSTRAINT `fk_market_data_crypto_symbol1` FOREIGN KEY(`crypto_id`) REFERENCES `crypto_symbol` (`crypto_id`), " +
                "CONSTRAINT `fk_market_data_fiat_symbol1` FOREIGN KEY(`fiat_id`) REFERENCES `fiat_symbol` (`fiat_id`))");

            // Populate helper tables with necessary data
            for(int i = -12; i <= 12; i++)
            {
                MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                    $"INSERT INTO time_zone (time_zone.change) VALUES ({i})");
            }

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('BTC')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('ETH')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('ADA')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('ATOM')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('DOGE')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO fiat_symbol (fiat_symbol.symbol) VALUES ('EUR')");

            // Create a insertable value strings for conveniance.
            var mockUser = ModelMock.GetValidUserModel();
            var timeZoneKey = DbKeyConversionHelper.TimeZoneToDbKey(mockUser.TimeZone);
            var userValues = $"'{mockUser.Email}', {mockUser.Verified}, '{mockUser.Username}', '{mockUser.Avatar}', '{timeZoneKey}'";

            var mockMarketDataOld = ModelMock.GetValidMarketDataModelOld();
            var formatedDateOld = DateTimeFormatHelper.ToDbFormatAsString(mockMarketDataOld.CollectionTime);
            var cryptoKeyOld = DbKeyConversionHelper.CryptoSymbolToDbKey(mockMarketDataOld.CryptoSymbol);
            var fiatKeyOld = DbKeyConversionHelper.FiatSymbolToDbKey(mockMarketDataOld.FiatSymbol);
            var marketDataValuesOld = $"'{formatedDateOld}', {mockMarketDataOld.UnitValue}, {mockMarketDataOld.Change24h}, " +
                $"{mockMarketDataOld.Change7d}, {cryptoKeyOld}, {fiatKeyOld}";

            var mockMarketDataNew = ModelMock.GetValidMarketDataModelNew();
            var formatedDateNew = DateTimeFormatHelper.ToDbFormatAsString(mockMarketDataNew.CollectionTime);
            var cryptoKeyNew = DbKeyConversionHelper.CryptoSymbolToDbKey(mockMarketDataNew.CryptoSymbol);
            var fiatKeyNew = DbKeyConversionHelper.FiatSymbolToDbKey(mockMarketDataNew.FiatSymbol);
            var marketDataValuesNew = $"'{formatedDateNew}', {mockMarketDataNew.UnitValue}, {mockMarketDataNew.Change24h}, " +
                $"{mockMarketDataNew.Change7d}, {cryptoKeyNew}, {fiatKeyNew}";

            // Insert mock data
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO user ({DatabaseConstants.UserColumns}) VALUES ({userValues})");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({marketDataValuesOld})");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({marketDataValuesNew})");

            return dbServer;
        }

        /// <summary>
        /// Executes received query and returns true if any rows were found.
        /// </summary>
        /// <param name="connectionString">Database connection</param>
        /// <param name="query">Query to execute</param>
        /// <returns></returns>
        public static bool QueryHasRows(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);

            return result.HasRows;
        }

        /// <summary>
        /// Queries test database with received query and returns username column value.
        /// </summary>
        /// <param name="connectionString">Database connection</param>
        /// <param name="query">Query to execute</param>
        /// <returns>username value</returns>
        public static string GetUsernameValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("username");
            var value = result.GetFieldValue<string>(ordinal);

            return value;
        }

        /// <summary>
        /// Queries test database with received query and returns avatar_url column value.
        /// </summary>
        /// <param name="connectionString">Database connection</param>
        /// <param name="query">Query to execute</param>
        /// <returns>avatar_url value</returns>
        public static string GetAvatarValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("avatar");
            var value = result.GetFieldValue<string>(ordinal);

            return value;
        }


        /// <summary>
        /// Queries test database with received query and returns time_zone column value.
        /// </summary>
        /// <param name="connectionString">Database connection</param>
        /// <param name="query">Query to execute</param>
        /// <returns>time_zone value</returns>
        public static string GetTimeZoneValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("zone_id");
            var value = DbKeyConversionHelper.TimeZoneKeyToString(result.GetFieldValue<int>(ordinal));

            return value;
        }

        /// <summary>
        /// Queries test database with received query and returns is_verified column value.
        /// </summary>
        /// <param name="connectionString">Database connection</param>
        /// <param name="query">Query to execute</param>
        /// <returns>is_verified value</returns>
        public static string GetVerificationValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("verified");
            var value = result.GetFieldValue<int>(ordinal);

            return "" + value;
        }
    }
}
