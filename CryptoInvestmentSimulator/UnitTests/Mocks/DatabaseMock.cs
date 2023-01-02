using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Enums;
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

            // Creates a mock wallet table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `wallet` (" +
                "`wallet_id` int NOT NULL AUTO_INCREMENT, " +
                "`symbol` varchar(4) NOT NULL, " +
                "`balance` decimal(12,6) NOT NULL, " +
                "`user_id` int NOT NULL, " +
                "PRIMARY KEY (`wallet_id`), " +
                "KEY `fk_wallet_user1_idx` (`user_id`), " +
                "CONSTRAINT `fk_wallet_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`))");

            // Creates a mock status table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `status` (" +
                "`status_id` int NOT NULL AUTO_INCREMENT, " +
                "`name` varchar(10) NOT NULL, " +
                "PRIMARY KEY (`status_id`))");

            // Creates a mock leverage ratio table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `leverage_ratio` (" +
                "`ratio_id` int NOT NULL AUTO_INCREMENT, " +
                "`multiplier` int NOT NULL, " +
                "PRIMARY KEY (`ratio_id`))");

            // Creates a mock transaction table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `transaction` (" +
                "`transaction_id` int NOT NULL AUTO_INCREMENT, " +
                "`date_time` datetime NOT NULL, " +
                "`fiat_amount` decimal(12,6) NOT NULL, " +
                "`crypto_amount` decimal(12,6) NOT NULL, " +
                "`margin` decimal(12,6) DEFAULT NULL, " +
                "`ratio_id` int DEFAULT NULL, " +
                "`status_id` int NOT NULL, " +
                "`wallet_id` int NOT NULL, " +
                "`data_id` int NOT NULL, " +
                "PRIMARY KEY (`transaction_id`), " +
                "KEY `fk_transaction_leverage_ratio1_idx` (`ratio_id`), " +
                "KEY `fk_transaction_status1_idx` (`status_id`), " +
                "KEY `fk_transaction_wallet1_idx` (`wallet_id`), " +
                "KEY `fk_transaction_market_data1_idx` (`data_id`), " +
                "CONSTRAINT `fk_transaction_leverage_ratio1` FOREIGN KEY (`ratio_id`) REFERENCES `leverage_ratio` (`ratio_id`), " +
                "CONSTRAINT `fk_transaction_market_data1` FOREIGN KEY (`data_id`) REFERENCES `market_data` (`data_id`), " +
                "CONSTRAINT `fk_transaction_status1` FOREIGN KEY (`status_id`) REFERENCES `status` (`status_id`), " +
                "CONSTRAINT `fk_transaction_wallet1` FOREIGN KEY (`wallet_id`) REFERENCES `wallet` (`wallet_id`))");

            // Populate helper tables with necessary data
            for (int i = -12; i <= 12; i++)
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

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO status (status.name) VALUES ('Open')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO status (status.name) VALUES ('Closed')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO status (status.name) VALUES ('Liquidated')");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (1)");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (2)");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (5)");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (10)");

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

            var eurWallet = $"'{FiatEnum.EUR}', 10, {mockUser.Id}";
            var btcWallet = $"'{CryptoEnum.BTC}', 20, {mockUser.Id}";
            var ethWallet = $"'{CryptoEnum.ETH}', 30, {mockUser.Id}";
            var adaWallet = $"'{CryptoEnum.ADA}', 40, {mockUser.Id}";
            var atomWallet = $"'{CryptoEnum.ATOM}', 50, {mockUser.Id}";
            var dogeWallet = $"'{CryptoEnum.DOGE}', 60, {mockUser.Id}";

            // Insert mock data
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO user ({DatabaseConstants.UserColumns}) VALUES ({userValues})");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({marketDataValuesOld})");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({marketDataValuesNew})");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({eurWallet})");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({btcWallet})");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({ethWallet})");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({adaWallet})");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({atomWallet})");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"), $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({dogeWallet})");

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

        public static decimal GetWalletBalance(string connectionString, string query)
        {
            var value = -1M;

            using (var reader = MySqlHelper.ExecuteReader(connectionString, query))
            {
                while (reader.Read())
                {
                    var ordinal = reader.GetOrdinal("balance");
                    value = reader.GetFieldValue<decimal>(ordinal);
                }
            }

            return value;
        }
    }
}
