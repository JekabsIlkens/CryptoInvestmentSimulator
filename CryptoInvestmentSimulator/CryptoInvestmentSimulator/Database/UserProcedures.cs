using CryptoInvestmentSimulator.Constants;
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
            var user = new UserModel();

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT * FROM users WHERE email = '{email}'", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.UserId = reader.GetInt32("user_id");
                        user.Username = reader.GetString("username");
                        user.Email = reader.GetString("email");
                        user.AvatarUrl = reader.GetString("avatar_url");
                        user.TimeZone = reader.GetString("time_zone");
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
                throw new ArgumentNullException($"Model property {nameof(userModel.Email)} is null or empty!");
            }

            if (!DoesUserExist(userModel.Email))
            {
                var valuesString = $"'{userModel.Username}', '{userModel.Email}', '{userModel.AvatarUrl}', '{userModel.TimeZone}'";

                using (var connection = context.GetConnection())
                {
                    connection.Open();
                    MySqlCommand command = new($"INSERT INTO users ({DatabaseConstants.UserColumns}) VALUES ({valuesString})", connection);
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
                throw new ArgumentNullException($"Received {nameof(email)} is null or empty!");
            }

            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE users SET username = '{username}' WHERE email = '{email}'", connection);
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
                throw new ArgumentNullException($"Received {nameof(avatar)} is null or empty!");
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE users SET avatar_url = '{avatar}' WHERE email = '{email}'", connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates given users timezone.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="timezone"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateTimeZone(string email, string timezone)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException($"Received {nameof(timezone)} is null or empty!");
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE users SET time_zone = '{timezone}' WHERE email = '{email}'", connection);
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
                throw new ArgumentNullException($"Received {nameof(email)} is null or empty!");
            }

            if (!IsUserVerified(email))
            {
                using (var connection = context.GetConnection())
                {
                    connection.Open();
                    MySqlCommand command = new($"UPDATE users SET is_verified = 1 WHERE email = '{email}'", connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Checks current status of users verification.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User verification status</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool IsUserVerified(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException($"Received {nameof(email)} is null or empty!");
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT is_verified FROM users WHERE email = '{email}'", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(reader.GetValue(reader.GetOrdinal("is_verified")).ToString() == "1")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Queries database for given user to see if it exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User existance status</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private bool DoesUserExist(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException($"Received {nameof(email)} is null or empty!");
            }

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new ($"SELECT * FROM users WHERE email = '{email}'", connection);

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
