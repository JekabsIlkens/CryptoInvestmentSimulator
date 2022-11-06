using MySql.Data.MySqlClient;

namespace CryptoInvestmentSimulator.Database
{
    public class DatabaseContext
    {
        public string ConnectionString { get; set; }

        public DatabaseContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }

            return new MySqlConnection(ConnectionString);
        }
    }
}
