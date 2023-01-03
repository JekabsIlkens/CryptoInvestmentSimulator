using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using MySql.Server;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class InvestmentProceduresTests
    {
        private static readonly MySqlServer mockServer = DatabaseInstanceMock.CreateMockDatabase();
        private static readonly string mockConnection = mockServer.GetConnectionString("mockdb");
        private static readonly DatabaseContext mockContext = new DatabaseContext(mockConnection);

        /// <summary>
        /// Tests if InsertNewPosition successfuly inserts a valid position model.
        /// </summary>
        [Fact]
        public void InsertNewPosition_OpenBitcoinPosition_PositionInserted()
        {
            // Arrange
            var procedures = new InvestmentProcedures(mockContext);

            var newPosition = ModelMock.GetOldBitcoinPositionWithoutLeverage();
            newPosition.DateTime = DateTime.Now;

            var positionsBefore = procedures.GetAllOpenPositions(1, CryptoEnum.BTC);

            // Act
            procedures.InsertNewPosition(newPosition);
            var positionsAfter = procedures.GetAllOpenPositions(1, CryptoEnum.BTC);

            mockServer.ShutDown();

            // Assert
            Assert.NotEqual(positionsBefore.Count, positionsAfter.Count);
            Assert.Single(positionsBefore);
            Assert.Equal(2, positionsAfter.Count);
        }

        /// <summary>
        /// Tests if GetAllOpenPositions correctly returns only open positions.
        /// </summary>
        [Fact]
        public void GetAllOpenPositions_OneOpenEthPosition_ReturnsPositionModel()
        {
            // Arrange
            var procedures = new InvestmentProcedures(mockContext);

            var existingPosition = ModelMock.GetOldBitcoinPositionWithoutLeverage();

            // Act
            var result = procedures.GetAllOpenPositions(1, CryptoEnum.BTC);

            mockServer.ShutDown();

            // Assert
            Assert.Single(result);
            Assert.Equal(existingPosition.Id, result[0].Id);
        }

        /// <summary>
        /// Tests if GetAllOpenLeveragedPositions correctly returns only open leveraged positions.
        /// </summary>
        [Fact]
        public void GetAllOpenLeveragedPositions_OneOpenLeveragedEthPosition_ReturnsPositionModel()
        {
            // Arrange
            var procedures = new InvestmentProcedures(mockContext);

            var existingPosition = ModelMock.GetNewEtheriumPositionWithLeverage();

            // Act
            var result = procedures.GetAllOpenPositions(1, CryptoEnum.ETH);

            mockServer.ShutDown();

            // Assert
            Assert.Single(result);
            Assert.Equal(existingPosition.Id, result[0].Id);
            Assert.NotEqual(1, result[0].Leverage);
        }

        /// <summary>
        /// Tests if GetPositionsUnitValue correctly returns unit value asociated to position.
        /// </summary>
        [Fact]
        public void GetPositionsUnitValue_OpenEthPosition_ReturnsAsociatedUnitValue()
        {
            // Arrange
            var procedures = new InvestmentProcedures(mockContext);

            var positionId = ModelMock.GetNewEtheriumPositionWithLeverage().Id;
            var expectedUnitValue = ModelMock.GetValidNewMarketDataModel().UnitValue;

            // Act
            var result = procedures.GetPositionsUnitValue(positionId);

            mockServer.ShutDown();

            // Assert
            Assert.Equal(expectedUnitValue, result);
        }

        /// <summary>
        /// Tests if UpdatePositionStatus successfuly closes an opened position.
        /// </summary>
        [Fact]
        public void UpdatePositionStatus_OpenPositionToClosed_PositionStatusUpdated()
        {
            // Arrange
            var procedures = new InvestmentProcedures(mockContext);

            var openPositionsBefore = procedures.GetAllOpenPositions(1, CryptoEnum.BTC);

            // Act
            procedures.UpdatePositionStatus(openPositionsBefore[0].Id, (int)StatusEnum.Closed);
            var openPositionsAfter = procedures.GetAllOpenPositions(1, CryptoEnum.BTC);

            mockServer.ShutDown();

            // Assert
            Assert.NotEqual(openPositionsBefore.Count, openPositionsAfter.Count);
        }
    }
}
