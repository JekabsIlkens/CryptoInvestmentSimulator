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
        /// Queries database for a specific user by email address.
        /// </summary>
        /// <param name="email">Email of requested user.</param>
        /// <returns>
        /// Filled <see cref="UserModel"/>.
        /// </returns>
        public UserModel GetUserDetails(string email)
        {
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
                        user.TimeZone = DbKeyConversionHelper.TimeZoneKeyToString(reader.GetInt32("zone_id"));
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// Checks if given user already exists in database. If not - inserts user details into database.
        /// </summary>
        /// <param name="userModel">Filled model with new users data.</param>
        public void InsertNewUser(UserModel userModel)
        {
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
        /// Updates username for given user.
        /// </summary>
        /// <param name="userId">Target users id.</param>
        /// <param name="username">New username.</param>
        public void UpdateUsername(int userId, string username)
        {
            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET username = '{username}' WHERE user_id = {userId}", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates avatar image url for given user.
        /// </summary>
        /// <param name="userId">Target users id.</param>
        /// <param name="avatarUrl">New avatar image url.</param>
        public void UpdateAvatar(int userId, string avatarUrl)
        {
            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET avatar = '{avatarUrl}' WHERE user_id = {userId}", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates time zone for given user.
        /// </summary>
        /// <param name="userId">Target users id.</param>
        /// <param name="timeZone">New time zone.</param>
        public void UpdateTimeZone(int userId, int timeZone)
        {
            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET zone_id = {timeZone} WHERE user_id = {userId}", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates verification status for given user.
        /// </summary>
        /// <param name="userId">Target users id.</param>
        public void UpdateVerification(int userId)
        {
            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE user SET verified = 1 WHERE user_id = {userId}", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Creates wallets with initial balances for a new user after verification process has been completed.
        /// </summary>
        /// <param name="userId">Target users id.</param>
        public void CreateWalletsForUser(int userId)
        {
            using (var connection = context.GetConnection())
            {
                connection.Open();

                MySqlCommand command;

                var values = $"'{FiatEnum.EUR}', {BusinessRuleConstants.InitialCapital}, {userId}";
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

        /// <summary>
        /// Creates a list filled with all verified user ids.
        /// </summary>
        /// <returns>
        /// Filled list of verified user ids.
        /// </returns>
        public List<int> GetAllVerifiedUserIds()
        {
            var userIdList = new List<int>();

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT user_id FROM user WHERE verified = 1", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userIdList.Add(reader.GetInt32("user_id"));
                    }
                }
            }

            return userIdList;
        }

        /// <summary>
        /// Queries database for given user to see if it exists.
        /// </summary>
        /// <param name="email">Requested users email.</param>
        /// <returns>
        /// True if exists, False if doesn't.
        /// </returns>
        private bool DoesUserExist(string email)
        {
            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT * FROM user WHERE email = '{email}'", connection);

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
    }
}
