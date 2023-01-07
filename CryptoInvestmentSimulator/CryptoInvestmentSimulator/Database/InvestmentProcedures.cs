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

		/// <summary>
		/// Inserts a filled <see cref="PositionModel"/> into database.
		/// </summary>
		/// <param name="positionModel">Filled position model.</param>
		public void InsertNewPosition(PositionModel positionModel)
		{
			var formattedDateTime = DateTimeFormatHelper.ToDbFormatAsString(positionModel.DateTime);

			string valuesString = $"'{formattedDateTime}', {positionModel.FiatAmount}, {positionModel.CryptoAmount}, {positionModel.Margin}, " +
					$"{positionModel.Leverage}, {positionModel.Status}, {positionModel.Wallet}, {positionModel.Data}";

			string commandString = $"INSERT INTO position ({DatabaseConstants.PositionColumns}) VALUES ({valuesString})";

			using (MySqlConnection connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new(commandString, connection);
				command.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Creates a list of all open positions for given user.
		/// </summary>
		/// <param name="userId">Position owner.</param>
		/// <returns>
		/// List of filled <see cref="PositionModel"/>s.
		/// </returns>
		public List<PositionModel> GetAllOpenPositions(int userId)
		{
			var modelList = new List<PositionModel>();

			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new
					($"SELECT position.position_id, wallet.user_id, market_data.crypto_id, market_data.crypto_id, position.date_time, position.fiat_amount, " +
					$"position.crypto_amount, position.ratio_id, position.margin, position.status_id " +
					$"FROM position " +
					$"INNER JOIN wallet ON position.wallet_id = wallet.wallet_id " +
					$"INNER JOIN market_data ON position.data_id = market_data.data_id " +
					$"WHERE wallet.user_id = {userId} AND position.status_id = 1 " +
					$"ORDER BY position.date_time DESC",
					connection);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var model = new PositionModel
						{
							Id = (int)reader.GetValue(reader.GetOrdinal("position_id")),
							BoughtCrypto = (int)reader.GetValue(reader.GetOrdinal("crypto_id")),
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

		/// <summary>
		/// Creates a list of all open positions for given user for specific cryptocurrency.
		/// </summary>
		/// <param name="userId">Position owner.</param>
		/// <param name="crypto">Specified crypto.</param>
		/// <returns>
		/// List of filled <see cref="PositionModel"/>s.
		/// </returns>
		public List<PositionModel> GetAllOpenSpecificCryptoPositions(int userId, CryptoEnum crypto)
		{
			var modelList = new List<PositionModel>();

			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new
					($"SELECT position.position_id, wallet.user_id, market_data.crypto_id, position.date_time, position.fiat_amount, " +
					$"position.crypto_amount, position.ratio_id, position.margin, position.status_id " +
					$"FROM position " +
					$"INNER JOIN wallet ON position.wallet_id = wallet.wallet_id " +
					$"INNER JOIN market_data ON position.data_id = market_data.data_id " +
					$"WHERE wallet.user_id = {userId} AND market_data.crypto_id = {(int)crypto} AND position.status_id = 1 " +
					$"ORDER BY position.date_time DESC", 
					connection);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var model = new PositionModel
						{
							Id = (int)reader.GetValue(reader.GetOrdinal("position_id")),
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

		/// <summary>
		/// Creates a list of all open leveraged positions for given user.
		/// </summary>
		/// <param name="userId">Position owner.</param>
		/// <returns>
		/// List of filled <see cref="LiquidationModel"/>s.
		/// </returns>
		public List<LiquidationModel> GetAllOpenLeveragedPositions(int userId)
		{
			var modelList = new List<LiquidationModel>();

			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new
					($"SELECT wallet.user_id, position.position_id, position.fiat_amount, position.crypto_amount, " +
					$"position.margin, position.ratio_id, market_data.unit_value, market_data.crypto_id " +
					$"FROM position " +
					$"INNER JOIN wallet ON position.wallet_id = wallet.wallet_id " +
					$"INNER JOIN market_data ON position.data_id = market_data.data_id " +
					$"WHERE wallet.user_id = {userId} AND position.status_id = 1 AND position.ratio_id > 1",
					connection);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var model = new LiquidationModel
						{
							positionId = (int)reader.GetValue(reader.GetOrdinal("position_id")),
							FiatAmount = (decimal)reader.GetValue(reader.GetOrdinal("fiat_amount")),
							CryptoAmount = (decimal)reader.GetValue(reader.GetOrdinal("crypto_amount")),
							MarginAmount = (decimal)reader.GetValue(reader.GetOrdinal("margin")),
							RatioId = (int)reader.GetValue(reader.GetOrdinal("ratio_id")),
							UnitValue = (decimal)reader.GetValue(reader.GetOrdinal("unit_value")),
							CryptoId = (int)reader.GetValue(reader.GetOrdinal("crypto_id"))
						};

						modelList.Add(model);
					}
				}
			}

			return modelList;
		}

		/// <summary>
		/// Returns the unit value of a given position (unit value at the time of opening position).
		/// </summary>
		/// <param name="positionId">Specific position.</param>
		/// <returns>
		/// Unit value asociated with position.
		/// </returns>
		public decimal GetPositionsUnitValue(int positionId)
		{
			var unitValue = 0M;

			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new
					($"SELECT position.position_id, market_data.unit_value " +
					$"FROM position " +
					$"INNER JOIN market_data ON position.data_id = market_data.data_id " +
					$"WHERE position_id = {positionId}",
					connection);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						unitValue = (decimal)reader.GetValue(reader.GetOrdinal("unit_value"));
					}
				}
			}

			return unitValue;
		}

		/// <summary>
		/// Updates status of specific position to specified status.
		/// </summary>
		/// <param name="positionId">Position to update.</param>
		/// <param name="newStatusId">New status id.</param>
		public void UpdatePositionStatus(int positionId, int newStatusId)
		{
			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new($"UPDATE position SET status_id = {newStatusId} WHERE position_id = {positionId}", connection);
				command.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Collects all existing positions a user owns and calls
		/// delete statement on each position.
		/// </summary>
		/// <param name="userId">Target user.</param>
		public void DeleteUsersPositions(int userId)
		{
			var positionIds = GetAllPositionIds(userId);
			MySqlCommand command;

			using (var connection = context.GetConnection())
			{
				connection.Open();

				foreach(var id in positionIds)
				{
					command = new($"DELETE FROM position WHERE position_id = {id}", connection);
					command.ExecuteNonQuery();
				}
			}

		}

		/// <summary>
		/// Collects all existing position ids that a user owns.
		/// </summary>
		/// <param name="userId">Position owner.</param>
		/// <returns>
		/// Int list of position ids.
		/// </returns>
		private List<int> GetAllPositionIds(int userId)
		{
			var idList = new List<int>();

			using (var connection = context.GetConnection())
			{
				connection.Open();
				MySqlCommand command = new
					($"SELECT position.position_id, wallet.user_id " +
					$"FROM position " +
					$"INNER JOIN wallet ON position.wallet_id = wallet.wallet_id " +
					$"WHERE wallet.user_id = {userId}",
					connection);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						idList.Add((int)reader.GetValue(reader.GetOrdinal("position_id")));
					}
				}
			}

			return idList;
		}
	}
}
