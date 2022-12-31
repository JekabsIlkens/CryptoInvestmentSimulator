using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Helpers;
using CryptoInvestmentSimulator.Models.ViewModels;
using MySql.Data.MySqlClient;

namespace CryptoInvestmentSimulator.Database
{
	public class InvestmentProcedures
	{
		private readonly DatabaseContext context;

		public InvestmentProcedures(DatabaseContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public void InsertNewPosition(PositionModel positionModel)
		{
			if (positionModel == null)
			{
				throw new ArgumentNullException(nameof(positionModel));
			}

			var formattedDateTime = DateTimeFormatHelper.ToDbFormatAsString(positionModel.DateTime);
			string valuesString, commandString;

			if (positionModel.Leverage == 0)
			{
				valuesString = $"'{formattedDateTime}', {positionModel.FiatAmount}, {positionModel.CryptoAmount}, " +
					$"{positionModel.Status}, {positionModel.Wallet}, {positionModel.Data}";

				commandString = $"INSERT INTO transaction ({DatabaseConstants.TransactionColumnsNoLeverage}) VALUES ({valuesString})";

			}
			else
			{
				valuesString = $"'{formattedDateTime}', {positionModel.FiatAmount}, {positionModel.CryptoAmount}, {positionModel.Margin}, " +
					$"{positionModel.Leverage}, {positionModel.Status}, {positionModel.Wallet}, {positionModel.Data}";

				commandString = $"INSERT INTO transaction ({DatabaseConstants.TransactionColumnsLeverage}) VALUES ({valuesString})";
			}

			using (MySqlConnection connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new(commandString, connection);
				command.ExecuteNonQuery();
			}
		}

		public List<PositionModel> GetAllActivePositions(int userId, CryptoEnum crypto)
		{
			var modelList = new List<PositionModel>();

			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new
					($"SELECT wallet.user_id, market_data.crypto_id, transaction.date_time, transaction.fiat_amount, " +
					$"transaction.crypto_amount, transaction.ratio_id, transaction.margin, transaction.status_id " +
					$"FROM cisdb.transaction " +
					$"INNER JOIN cisdb.wallet ON transaction.wallet_id = wallet.wallet_id " +
					$"INNER JOIN cisdb.market_data ON transaction.data_id = market_data.data_id " +
					$"WHERE wallet.user_id = {userId} AND market_data.crypto_id = {(int)crypto} AND transaction.status_id = 1 " +
					$"ORDER BY transaction.date_time DESC", 
					connection);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var model = new PositionModel
						{
							DateTime = (DateTime)reader.GetValue(reader.GetOrdinal("date_time")),
							FiatAmount = (decimal)reader.GetValue(reader.GetOrdinal("fiat_amount")),
							CryptoAmount = (decimal)reader.GetValue(reader.GetOrdinal("crypto_amount")),
							Leverage = (int)reader.GetValue(reader.GetOrdinal("ratio_id")),
							Margin = (decimal)reader.GetValue(reader.GetOrdinal("margin"))
						};

						modelList.Add(model);
					}
				}
			}

			return modelList;
		}

		public int GetUserWalletId(int userId, FiatEnum fiat)
		{
			if (userId < 1) throw new ArgumentException(nameof(userId));

			var walletId = -1;

			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new($"SELECT * FROM wallet WHERE user_id = {userId} AND symbol = '{fiat}'", connection);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						walletId = (int)reader.GetValue(reader.GetOrdinal("wallet_id"));
					}
				}
			}

			return walletId;
		}
	}
}
