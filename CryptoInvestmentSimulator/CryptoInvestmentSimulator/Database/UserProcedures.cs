using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Models;
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
        /// Checks if user already exists. If not, inserts it into database.
        /// </summary>
        /// <param name="userModel"></param>
        public void InsertNewUser(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email))
            {
                throw new ArgumentNullException($"Model property {nameof(userModel.Email)} is null or empty!");
            }

            if (!DoesUserExist(userModel.Email))
            {
                var valuesString = $"'{userModel.Username}', '{userModel.Email}', '{userModel.AvatarUrl}'";

                using (MySqlConnection connection = context.GetConnection())
                {
                    connection.Open();
                    MySqlCommand command = new($"INSERT INTO users ({DatabaseConstants.UserColumns}) VALUES ({valuesString})", connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates users verification status.
        /// </summary>
        /// <param name="emailAddress"></param>
        public void UpdateUserVerification(string emailAddress)
        {
            if (emailAddress != null && !IsUserVerified(emailAddress))
            {
                using (MySqlConnection connection = context.GetConnection())
                {
                    connection.Open();
                    MySqlCommand command = new($"UPDATE users SET is_verified = 1 WHERE email = '{emailAddress}'", connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Checks if user has verified their email address.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns>Bool</returns>
        public bool IsUserVerified(string emailAddress)
        {
            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT is_verified FROM users WHERE email = '{emailAddress}'", connection);

                using (MySqlDataReader reader = command.ExecuteReader())
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

        private bool DoesUserExist(string emailAddress)
        {
            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new ($"SELECT * FROM users WHERE email = '{emailAddress}'", connection);

                using (MySqlDataReader reader = command.ExecuteReader())
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
