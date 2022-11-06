using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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
            if (userModel.EmailAddress != null && !DoesUserExist(userModel.EmailAddress))
            {
                var valuesString = $"'{userModel.FirstName}', '{userModel.LastName}', '{userModel.EmailAddress}', '{userModel.AvatarUrl}'";

                using (MySqlConnection connection = context.GetConnection())
                {
                    connection.Open();
                    MySqlCommand command = new($"INSERT INTO users ({DatabaseConstants.UserColumns}) VALUES ({valuesString})", connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        private bool DoesUserExist(string emailAddress)
        {
            using (MySqlConnection connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new ($"SELECT * FROM users WHERE email_address = '{emailAddress}'", connection);

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
