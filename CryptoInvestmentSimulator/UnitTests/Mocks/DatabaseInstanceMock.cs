using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Helpers;
using MySql.Data.MySqlClient;
using MySql.Server;

namespace UnitTests.Mocks
{
    public class DatabaseInstanceMock
    {
        /// <summary>
        /// Creates a mock database for testing purposes that imitates the real CIS database.
        /// Tables are populated with fake data to be used in different test scenarios.
        /// </summary>
        /// <returns>Configured <see cref="MySqlServer.Instance"/></returns>
        public static MySqlServer CreateMockDatabase()
        {
            // Sets up a new mock sql server instance.
            MySqlServer dbServer = MySqlServer.Instance;

            // Starts the mock server.
            dbServer.StartServer();

            // Creates a mock database.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString(), "CREATE DATABASE mockdb;");

            // Populates mockdb with imitations of all CIS database tables.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.TimeZoneTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.UserTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.CryptoSymbolTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.FiatSymbolTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.MarketDataTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.WalletTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.StatusTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.LeverageRatioTable);
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), DatabaseScriptMock.PositionTable);

            // Populates the static helper tables with required records.
            for (int i = -12; i <= 12; i++)
            {
                MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO time_zone (time_zone.change) VALUES ({i})");
            }

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('BTC')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('ETH')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('ADA')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('ATOM')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO crypto_symbol (crypto_symbol.symbol) VALUES ('DOGE')");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO fiat_symbol (fiat_symbol.symbol) VALUES ('EUR')");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO status (status.name) VALUES ('Open')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO status (status.name) VALUES ('Closed')");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO status (status.name) VALUES ('Liquidated')");

            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (1)");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (2)");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (5)");
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), $"INSERT INTO leverage_ratio (leverage_ratio.multiplier) VALUES (10)");

            // Populates user table with records.
            var mockVerifiedUser = ModelMock.GetValidVerifiedUserModel();
            var timeZoneKey1 = DatabaseKeyConversionHelper.TimeZoneToDbKey(mockVerifiedUser.TimeZone);
            var verifiedUserValues = $"'{mockVerifiedUser.Email}', {mockVerifiedUser.Verified}, " +
                $"'{mockVerifiedUser.Username}', '{mockVerifiedUser.Avatar}', '{timeZoneKey1}'";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO user ({DatabaseConstants.UserColumns}) VALUES ({verifiedUserValues})");

            // Populates market data table with records.
            var mockMarketDataOld = ModelMock.GetValidOldMarketDataModel();
            var formatedDateOld = DateTimeFormatHelper.ToDbFormatAsString(mockMarketDataOld.CollectionTime);
            var cryptoKeyOld = DatabaseKeyConversionHelper.CryptoSymbolToDbKey(mockMarketDataOld.CryptoSymbol);
            var fiatKeyOld = DatabaseKeyConversionHelper.FiatSymbolToDbKey(mockMarketDataOld.FiatSymbol);
            var marketDataValuesOld = $"'{formatedDateOld}', {mockMarketDataOld.UnitValue}, {mockMarketDataOld.Change24h}, " +
                $"{mockMarketDataOld.Change7d}, {cryptoKeyOld}, {fiatKeyOld}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({marketDataValuesOld})");

            var mockMarketDataNew = ModelMock.GetValidNewMarketDataModel();
            var formatedDateNew = DateTimeFormatHelper.ToDbFormatAsString(mockMarketDataNew.CollectionTime);
            var cryptoKeyNew = DatabaseKeyConversionHelper.CryptoSymbolToDbKey(mockMarketDataNew.CryptoSymbol);
            var fiatKeyNew = DatabaseKeyConversionHelper.FiatSymbolToDbKey(mockMarketDataNew.FiatSymbol);
            var marketDataValuesNew = $"'{formatedDateNew}', {mockMarketDataNew.UnitValue}, {mockMarketDataNew.Change24h}, " +
                $"{mockMarketDataNew.Change7d}, {cryptoKeyNew}, {fiatKeyNew}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO market_data ({DatabaseConstants.MarketDataColumns}) VALUES ({marketDataValuesNew})");

            // Populates wallet table with records.
            var eurWallet = $"'{FiatEnum.EUR}', 10, {mockVerifiedUser.Id}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({eurWallet})");

            var btcWallet = $"'{CryptoEnum.BTC}', 20, {mockVerifiedUser.Id}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({btcWallet})");

            var ethWallet = $"'{CryptoEnum.ETH}', 30, {mockVerifiedUser.Id}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({ethWallet})");

            var adaWallet = $"'{CryptoEnum.ADA}', 40, {mockVerifiedUser.Id}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({adaWallet})");

            var atomWallet = $"'{CryptoEnum.ATOM}', 50, {mockVerifiedUser.Id}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({atomWallet})");

            var dogeWallet = $"'{CryptoEnum.DOGE}', 60, {mockVerifiedUser.Id}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({dogeWallet})");

            // Populates position table with records.
            var positionOne = ModelMock.GetOldBitcoinPositionWithoutLeverage();
            var formatedPosOneDate = DateTimeFormatHelper.ToDbFormatAsString(positionOne.DateTime);
            var withoutLeverage = $"'{formatedPosOneDate}', {positionOne.FiatAmount}, {positionOne.CryptoAmount}, {positionOne.Margin}, " +
                $"{positionOne.Leverage}, {positionOne.Status}, {positionOne.Wallet}, {positionOne.Data}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"), 
                $"INSERT INTO position ({DatabaseConstants.PositionColumns}) VALUES ({withoutLeverage})");

            var positionTwo = ModelMock.GetNewEtheriumPositionWithLeverage();
            var formatedPosTwoDate = DateTimeFormatHelper.ToDbFormatAsString(positionTwo.DateTime);
            var withLeverage = $"'{formatedPosTwoDate}', {positionTwo.FiatAmount}, {positionTwo.CryptoAmount}, {positionTwo.Margin}, " +
                $"{positionTwo.Leverage}, {positionTwo.Status}, {positionTwo.Wallet}, {positionTwo.Data}";
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("mockdb"),
                $"INSERT INTO position ({DatabaseConstants.PositionColumns}) VALUES ({withLeverage})");


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
        /// Queries mockdb database with received query and returns username column value.
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
        /// Queries mockdb database with received query and returns avatar_url column value.
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
        /// Queries mockdb database with received query and returns time_zone column value.
        /// </summary>
        /// <param name="connectionString">Database connection</param>
        /// <param name="query">Query to execute</param>
        /// <returns>time_zone value</returns>
        public static string GetTimeZoneValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("zone_id");
            var value = DatabaseKeyConversionHelper.TimeZoneKeyToString(result.GetFieldValue<int>(ordinal));

            return value;
        }

        /// <summary>
        /// Queries mockdb database with received query and returns is_verified column value.
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
