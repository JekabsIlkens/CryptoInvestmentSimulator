using MySql.Data.MySqlClient;
using MySql.Server;

namespace UnitTests.Mocks
{
    public class DatabaseMock
    {
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

        public static bool QueryHasRows(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);

            return result.HasRows;
        }

        public static string GetUsernameValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("username");
            var value = result.GetFieldValue<string>(ordinal);

            return value;
        }

        public static string GetAvatarValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("avatar_url");
            var value = result.GetFieldValue<string>(ordinal);

            return value;
        }

        public static string GetTimeZoneValue(string connectionString, string query)
        {
            var result = MySqlHelper.ExecuteReader(connectionString, query);
            result.Read();

            var ordinal = result.GetOrdinal("time_zone");
            var value = result.GetFieldValue<string>(ordinal);

            return value;
        }

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
