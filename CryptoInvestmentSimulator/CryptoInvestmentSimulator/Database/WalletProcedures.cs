using CryptoInvestmentSimulator.Models.ViewModels;
using MySql.Data.MySqlClient;

namespace CryptoInvestmentSimulator.Database
{
    public class WalletProcedures
    {
        private readonly DatabaseContext context;

        public WalletProcedures(DatabaseContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Collects all wallet balances into a <see cref="WalletModel"/> for given user.
        /// </summary> 
        /// <param name="userId">User id / Walled owner</param>
        /// <returns>Filled <see cref="WalletModel"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public WalletModel GetUsersWalletBalances(int userId)
        {
            if(userId < 1) throw new ArgumentException(nameof(userId));

            WalletModel walletModel = new WalletModel();

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT * FROM wallet WHERE user_id = {userId}", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var currencySymbol = reader.GetValue(reader.GetOrdinal("symbol")).ToString();
                        var currencyBalance = reader.GetValue(reader.GetOrdinal("balance")).ToString();
                        var decimalBalance = decimal.Parse(currencyBalance);

                        switch (currencySymbol)
                        {
                            case "EUR": 
                                walletModel.EuroAmount = decimalBalance;
                                continue;
                            case "BTC":
                                walletModel.BitcoinAmount = decimalBalance;
                                continue;
                            case "ETH":
                                walletModel.EtheriumAmount = decimalBalance;
                                continue;
                            case "ADA":
                                walletModel.CardanoAmount = decimalBalance;
                                continue;
                            case "ATOM":
                                walletModel.CosmosAmount = decimalBalance;
                                continue;
                            case "DOGE":
                                walletModel.DogecoinAmount = decimalBalance;
                                continue;
                            default: continue;
                        }
                    }
                }
            }

            return walletModel;
        }

        /// <summary>
        /// Collects a specified wallet balance for given user.
        /// </summary> 
        /// <param name="userId">User id / Walled owner</param>
        /// <returns>Current balance of wallet</returns>
        /// <exception cref="ArgumentException"></exception>
        public decimal GetSpecificWalletBalance(int userId, string symbol)
        {
            if (userId < 1) throw new ArgumentException(nameof(userId));

            var currencyBalance = 0M;

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"SELECT * FROM wallet WHERE user_id = {userId} AND symbol = '{symbol}'", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currencyBalance = (decimal)reader.GetValue(reader.GetOrdinal("balance"));
                    }
                }
            }

            return currencyBalance;
        }

        /// <summary>
        /// Updates a specified users wallet balance for specified currency.
        /// </summary>
        /// <param name="userId">Wallet owner</param>
        /// <param name="symbol">Target currency</param>
        /// <param name="newBalance">New balance</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateUsersWalletBalance(int userId, string symbol, decimal newBalance)
        {
            if (userId < 1) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrWhiteSpace(symbol)) throw new ArgumentNullException(nameof(symbol));
            if (newBalance < 0) throw new ArgumentException(nameof(newBalance));

            using (var connection = context.GetConnection())
            {
                connection.Open();
                MySqlCommand command = new($"UPDATE wallet SET balance = {newBalance} WHERE user_id = {userId} AND symbol = '{symbol}'", connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
