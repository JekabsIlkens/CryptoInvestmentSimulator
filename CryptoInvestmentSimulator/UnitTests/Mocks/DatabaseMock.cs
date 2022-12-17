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

            // Creates a mock users table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `users` (" +
                "`user_id` int NOT NULL AUTO_INCREMENT, " +
                "`username` varchar(45) NOT NULL, " +
                "`email` varchar(45) NOT NULL, " +
                "`avatar_url` varchar(512) DEFAULT NULL, " +
                "`is_verified` int NOT NULL DEFAULT '0', " +
                "`time_zone` varchar(10) NOT NULL, " +
                "PRIMARY KEY (`user_id`))");

            // Creates a mock market_data table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `market_data` (" +
                "`data_id` int NOT NULL AUTO_INCREMENT, " +
                "`crypto_symbol` varchar(4) NOT NULL, " +
                "`fiat_symbol` varchar(4) NOT NULL, " +
                "`date_time` datetime NOT NULL, " +
                "`unit_value` decimal(12,6) NOT NULL, " +
                "`daily_change` decimal(6,2) NOT NULL, " +
                "`weekly_change` decimal(6,2) NOT NULL, " +
                "PRIMARY KEY (`data_id`))");

            // Create table column strings for conveniance.
            var userColumns = "`user_id`, `username`, `email`, `avatar_url`, `is_verified`, `time_zone`";
            var marketDataColumns = "`data_id`, `crypto_symbol`, `fiat_symbol`, `date_time`, `unit_value`, `daily_change`, `weekly_change`";

            // Create a insertable value strings for conveniance.
            var mockUser = ModelMock.GetValidUserModel();
            var userValues = $"{mockUser.Id}, '{mockUser.Username}', '{mockUser.Email}', '{mockUser.Avatar}', 0, '{mockUser.TimeZone}'";

            var mockMarketDataOld = ModelMock.GetValidMarketDataModelOld();
            var formatedDateOld = DateTimeFormatHelper.ToDbFormatAsString(mockMarketDataOld.CollectionDateTime);
            var marketDataValuesOld = $"1, '{mockMarketDataOld.CryptoSymbol}', '{mockMarketDataOld.FiatSymbol}', '{formatedDateOld}', " +
                $"{mockMarketDataOld.FiatPricePerUnit}, {mockMarketDataOld.PercentChange24h}, {mockMarketDataOld.PercentChange7d}";

            var mockMarketDataNew = ModelMock.GetValidMarketDataModelNew();
            var formatedDateNew = DateTimeFormatHelper.ToDbFormatAsString(mockMarketDataNew.CollectionDateTime);
            var marketDataValuesNew = $"2, '{mockMarketDataNew.CryptoSymbol}', '{mockMarketDataNew.FiatSymbol}', '{formatedDateNew}', " +
                $"{mockMarketDataNew.FiatPricePerUnit}, {mockMarketDataNew.PercentChange24h}, {mockMarketDataNew.PercentChange7d}";

            // Insert mock data
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO users ({userColumns}) VALUES ({userValues})");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO market_data ({marketDataColumns}) VALUES ({marketDataValuesOld})");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO market_data ({marketDataColumns}) VALUES ({marketDataValuesNew})");

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

            var ordinal = result.GetOrdinal("avatar_url");
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

            var ordinal = result.GetOrdinal("time_zone");
            var value = result.GetFieldValue<string>(ordinal);

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

            var ordinal = result.GetOrdinal("is_verified");
            var value = result.GetFieldValue<int>(ordinal);

            return "" + value;
        }
    }
}
