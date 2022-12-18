using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Models.ViewModels;
using FluentAssertions;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
	public class MarketDataProceduresTests
	{
		/// <summary>
		/// Tests if passing a MarketDataModel equal to null throws exception.
		/// </summary>
		[Fact]
		public void InsertNewMarketDataEntry_ParameterIsNull_Exception()
		{
			// Arrange
			var marketDataProcedures = new MarketDataProcedures(new DatabaseContext(DatabaseConstants.Access));
			var marketDataModel = new MarketDataModel();
			marketDataModel = null;

			// Act
			Action act = () => marketDataProcedures.InsertNewMarketDataEntry(marketDataModel);

			// Assert
			act.Should().ThrowExactly<ArgumentNullException>();
		}

		/// <summary>
		/// Tests if the latest market data record gets returned
		/// and not any older records.
		/// </summary>
		[Fact]
		public void GetLatestMarketData_DataExistsInDb_FilledMarketDataModel()
		{
			// Arrange
			var testServer = DatabaseMock.CreateDatabase();
			var mockConnection = testServer.GetConnectionString("test");
			var mockContext = new DatabaseContext(mockConnection);

			var procedures = new MarketDataProcedures(mockContext);
			var mockMarketData = ModelMock.GetValidMarketDataModelNew();

			// Act
			var result = procedures.GetLatestMarketData(CryptoEnum.BTC);
			testServer.ShutDown();

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
			var testServer = DatabaseMock.CreateDatabase();
			var mockConnection = testServer.GetConnectionString("test");
			var mockContext = new DatabaseContext(mockConnection);

			var procedures = new MarketDataProcedures(mockContext);
			var mockMarketDataOld = ModelMock.GetValidMarketDataModelOld();
			var mockMarketDataNew = ModelMock.GetValidMarketDataModelNew();

			// Act
			var result = procedures.GetPricePointHistory(CryptoEnum.BTC, 2, 1);
			testServer.ShutDown();

			// Assert
			Assert.Equal((double)mockMarketDataOld.UnitValue, result[0]);
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
			var testServer = DatabaseMock.CreateDatabase();
			var mockConnection = testServer.GetConnectionString("test");
			var mockContext = new DatabaseContext(mockConnection);

			var procedures = new MarketDataProcedures(mockContext);
			var mockMarketDataOld = ModelMock.GetValidMarketDataModelOld();
			var mockMarketDataNew = ModelMock.GetValidMarketDataModelNew();

			var oldDate = mockMarketDataOld.CollectionTime.ToString();
			var expectedOldDate = ((DateTimeOffset)DateTime.Parse(oldDate)).ToUnixTimeSeconds() * 1000;

			var newDate = mockMarketDataNew.CollectionTime.ToString();
			var expectedNewDate = ((DateTimeOffset)DateTime.Parse(newDate)).ToUnixTimeSeconds() * 1000;

			// Act
			var result = procedures.GetTimePointHistory(CryptoEnum.BTC, 2, 1);
			testServer.ShutDown();

			// Assert
			Assert.Equal(expectedOldDate, result[0]);
			Assert.Equal(expectedNewDate, result[1]);
		}
	}
}
