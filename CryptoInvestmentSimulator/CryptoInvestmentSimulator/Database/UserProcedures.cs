using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Helpers;
using CryptoInvestmentSimulator.Models.ViewModels;
using MySql.Data.MySqlClient;

namespace CryptoInvestmentSimulator.Database
{
    public class UserProcedures
    {
        private readonly DatabaseContext context;

        public UserProcedures(DatabaseContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Queries database for a specific user by email.
        /// </summary>
        /// <returns>Filled user model</returns>
        public UserModel GetUserDetails(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = new UserModel();

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT * FROM user WHERE email = '{email}'", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.Id = reader.GetInt32("user_id");
                        user.Email = reader.GetString("email");
                        user.Verified = reader.GetInt32("verified");
                        user.Username = reader.GetString("username");
                        user.Avatar = reader.GetString("avatar");

                        var timeZoneString = DbKeyConversionHelper.TimeZoneKeyToString(reader.GetInt32("zone_id"));
                        user.TimeZone = timeZoneString;
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// Checks if given user already exists in datbase. 
        /// If not - inserts user details into database.
        /// </summary>
        /// <param name="userModel"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void InsertNewUser(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email))
            {
                throw new ArgumentNullException(nameof(userModel.Email));
            }

            if (!DoesUserExist(userModel.Email))
            {
                var timeZoneKey = DbKeyConversionHelper.TimeZoneToDbKey(userModel.TimeZone);
                var valuesString = $"'{userModel.Email}', {userModel.Verified}, '{userModel.Username}', '{userModel.Avatar}', {timeZoneKey}";

                using (var connection = context.GetConnection())
                {
                    connection.Open();
                    MySqlCommand command = new($"INSERT INTO user ({DatabaseConstants.UserColumns}) VALUES ({valuesString})", connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates given users username.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateUsername(string email, string username)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET username = '{username}' WHERE email = '{email}'", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates given users avatar url.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="avatar"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateAvatar(string email, string avatar)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET avatar = '{avatar}' WHERE email = '{email}'", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates given users timezone.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="timezone"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateTimeZone(string email, int timezone)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET zone_id = {timezone} WHERE email = '{email}'", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Checks if given user is already verified.
        /// If not - updates given users verification status.
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateVerification(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET verified = 1 WHERE email = '{email}'", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Queries database for given user to see if it exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User existance status</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool DoesUserExist(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new ($"SELECT * FROM user WHERE email = '{email}'", connection);

                using (var reader = command.ExecuteReader())
                {                 
                    while (reader.Read())
                    {
                        if (reader.HasRows) return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Creates new wallets with initial balances for user
        /// after verification process has been done.
        /// </summary>
        /// <param name="email">Current users email</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void CreateWalletsForUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var userId = -1;
            var values = $"";

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT user_id FROM user WHERE email = '{email}'", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userId = (int)reader.GetValue(reader.GetOrdinal("user_id"));
                    }
                }

                values = $"'{FiatEnum.EUR}', 5000, {userId}";
                command = new($"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({values})", connection);
                command.ExecuteNonQuery();

                values = $"'{CryptoEnum.BTC}', 0, {userId}";
                command = new($"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({values})", connection);
                command.ExecuteNonQuery();

                values = $"'{CryptoEnum.ETH}', 0, {userId}";
                command = new($"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({values})", connection);
                command.ExecuteNonQuery();

                values = $"'{CryptoEnum.ADA}', 0, {userId}";
                command = new($"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({values})", connection);
                command.ExecuteNonQuery();

                values = $"'{CryptoEnum.ATOM}', 0, {userId}";
                command = new($"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({values})", connection);
                command.ExecuteNonQuery();

                values = $"'{CryptoEnum.DOGE}', 0, {userId}";
                command = new($"INSERT INTO wallet ({DatabaseConstants.WalletColumns}) VALUES ({values})", connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
