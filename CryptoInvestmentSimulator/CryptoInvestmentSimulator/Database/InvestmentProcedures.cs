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
