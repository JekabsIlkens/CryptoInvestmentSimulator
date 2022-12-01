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
            // Setup a new mock sql server instance.
            MySqlServer dbServer = MySqlServer.Instance;

            // Start the mock server.
            dbServer.StartServer();

            //Create a mock database.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString(), "CREATE DATABASE test;");

            //Create a mock users table.
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                "CREATE TABLE `users` (" +
                "`user_id` int NOT NULL AUTO_INCREMENT, " +
                "`username` varchar(45) NOT NULL, " +
                "`email` varchar(45) NOT NULL, " +
                "`avatar_url` varchar(512) DEFAULT NULL, " +
                "`is_verified` int NOT NULL DEFAULT '0', " +
                "`time_zone` varchar(10) NOT NULL, " +
                "PRIMARY KEY (`user_id`))");

            // Create a user table column string for conveniance.
            var columns = "`user_id`, `username`, `email`, `avatar_url`, `is_verified`, `time_zone`";

            // Create a insertable value string for conveniance.
            var mockUser = ModelMock.GetValidUserModel();
            var values = $"{mockUser.UserId}, '{mockUser.Username}', '{mockUser.Email}', '{mockUser.AvatarUrl}', 0, '{mockUser.TimeZone}'";

            //Insert data
            MySqlHelper.ExecuteNonQuery(dbServer.GetConnectionString("test"),
                $"INSERT INTO users ({columns}) VALUES ({values})");

            return dbServer;
        }

        /// <summary>
        /// Executes received query and returns true if any rows were found.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static bool QueryHasRows(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);

            return result.HasRows;
        }

        /// <summary>
        /// Queries test database with received query and returns username column value.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
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
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
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
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
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
        /// <param name="connectionString"></param>
        /// <param name="query"></param>
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
