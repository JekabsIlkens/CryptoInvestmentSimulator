using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class WalletProceduresTests
    {
        /// <summary>
        /// Tests if GetUsersWalletBalances returns correct balances for given user.
        /// </summary>
        [Fact]
        public void GetUsersWalletBalances_ExistingUserWithWallets_CorrectBalances()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new WalletProcedures(mockContext);

            // Act
            var result = procedures.GetUsersWalletBalances(1);

            // Assert
            Assert.Equal(10M, result.EuroAmount);
            Assert.Equal(20M, result.BitcoinAmount);
            Assert.Equal(30M, result.EtheriumAmount);
            Assert.Equal(40M, result.CardanoAmount);
            Assert.Equal(50M, result.CosmosAmount);
            Assert.Equal(60M, result.DogecoinAmount);
        }

        /// <summary>
        /// Tests if UpdateUsersWalletBalance correctly updates specified wallet contents.
        /// </summary>
        [Fact]
        public void UpdateUsersWalletBalance_ExistingUserWithWallets_UpdatedBalance()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new WalletProcedures(mockContext);

            // Act
            var previous = procedures.GetUsersWalletBalances(1);
            procedures.UpdateUsersWalletBalance(1, CryptoEnum.BTC.ToString(), 999M);
            var updated = procedures.GetUsersWalletBalances(1);

            // Assert
            Assert.Equal(20M, previous.BitcoinAmount);
            Assert.Equal(999M, updated.BitcoinAmount);
        }
    }
}
