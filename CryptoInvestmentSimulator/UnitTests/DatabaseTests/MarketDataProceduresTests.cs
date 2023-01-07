using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using MySql.Server;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
	public class MarketDataProceduresTests
	{
		private static readonly MySqlServer mockServer = DatabaseInstanceMock.CreateMockDatabase();
		private static readonly string mockConnection = mockServer.GetConnectionString("mockdb");
		private static readonly DatabaseContext mockContext = new DatabaseContext(mockConnection);

		/// <summary>
		/// Tests is new market data insertion method successfuly inserts
		/// a valid market data model.
		/// </summary>
		[Fact]
		public void InsertNewMarketDataEntry_ValidMarketDataModel_SuccessfulInsert()
		{
			// Arrange
			var procedures = new MarketDataProcedures(mockContext);

			var before = procedures.GetLatestMarketData(CryptoEnum.DOGE);

			var mockMarketData = ModelMock.GetValidNewMarketDataModel();
			mockMarketData.CryptoSymbol = CryptoEnum.DOGE.ToString();
			mockMarketData.UnitValue = 1111M;

			// Act
			procedures.InsertNewMarketDataEntry(mockMarketData);
			var after = procedures.GetLatestMarketData(CryptoEnum.DOGE);
			mockServer.ShutDown();

			// Assert
			Assert.Null(before.CryptoSymbol);
			Assert.Equal("DOGE", after.CryptoSymbol);
			Assert.NotEqual(before.UnitValue, after.UnitValue);
		}

		/// <summary>
		/// Tests if the latest market data record gets returned
		/// and not any older records.
		/// </summary>
		[Fact]
		public void GetLatestMarketData_DataExistsInDb_FilledMarketDataModel()
		{
			// Arrange
			var procedures = new MarketDataProcedures(mockContext);

			var mockMarketData = ModelMock.GetValidNewMarketDataModel();

			// Act
			var result = procedures.GetLatestMarketData(CryptoEnum.ETH);
			mockServer.ShutDown();

			// Assert
			Assert.Equal(mockMarketData.CryptoSymbol, result.CryptoSymbol);
			Assert.Equal(mockMarketData.FiatSymbol, result.FiatSymbol);
			Assert.Equal(mockMarketData.UnitValue, result.UnitValue);
			Assert.Equal(mockMarketData.Change24h, result.Change24h);
			Assert.Equal(mockMarketData.Change7d, result.Change7d);
		}

		/// <summary>
		/// Tests if correct amount of price points get returned
		/// and if price point data matches expected values and format.
		/// </summary>
		[Fact]
		public void GetPricePointHistory_TwoRecordsInDb_Returns2Values()
		{
			// Arrange
			var procedures = new MarketDataProcedures(mockContext);

			var mockMarketDataNew = ModelMock.GetValidNewMarketDataModel();
			mockMarketDataNew.CryptoSymbol = CryptoEnum.BTC.ToString();
			procedures.InsertNewMarketDataEntry(mockMarketDataNew);

			// Act
			var result = procedures.GetPricePointHistory(CryptoEnum.BTC, 2, 1);
			mockServer.ShutDown();

			// Assert
			Assert.Equal((double)0.025M, result[0]);
			Assert.Equal((double)mockMarketDataNew.UnitValue, result[1]);
		}

		/// <summary>
		/// Tests if correct amount of time points get returned
		/// and if time point data matches expected values and format.
		/// </summary>
		[Fact]
		public void GetTimePointHistory_TwoRecordsInDb_Returns2Values()
		{
			// Arrange
			var procedures = new MarketDataProcedures(mockContext);

			var mockMarketDataNew = ModelMock.GetValidNewMarketDataModel();
			mockMarketDataNew.CryptoSymbol = CryptoEnum.BTC.ToString();
			procedures.InsertNewMarketDataEntry(mockMarketDataNew);

			var oldDate = DateTime.Now.AddDays(-10).ToString();
			var expectedOldDate = ((DateTimeOffset)DateTime.Parse(oldDate)).ToUnixTimeSeconds() * 1000;

			var newDate = mockMarketDataNew.CollectionTime.ToString();
			var expectedNewDate = ((DateTimeOffset)DateTime.Parse(newDate)).ToUnixTimeSeconds() * 1000;

			// Act
			var result = procedures.GetTimePointHistory(CryptoEnum.BTC, 2, 1);
			mockServer.ShutDown();

			// Assert
			Assert.Equal(expectedOldDate, result[0]);
			Assert.Equal(expectedNewDate, result[1]);
		}
	}
}
