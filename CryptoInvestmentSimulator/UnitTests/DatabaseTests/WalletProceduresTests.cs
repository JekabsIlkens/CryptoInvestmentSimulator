using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using MySql.Server;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class WalletProceduresTests
    {
        private static readonly MySqlServer mockServer = DatabaseInstanceMock.CreateMockDatabase();
        private static readonly string mockConnection = mockServer.GetConnectionString("mockdb");
        private static readonly DatabaseContext mockContext = new DatabaseContext(mockConnection);

        /// <summary>
        /// Tests if GetUsersWalletBalances returns correct balances for given user.
        /// </summary>
        [Fact]
        public void GetUsersWalletBalances_ExistingUserWithWallets_CorrectBalances()
        {
            // Arrange
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
            var procedures = new WalletProcedures(mockContext);

            // Act
            var previousBalances = procedures.GetUsersWalletBalances(1);
            procedures.UpdateUsersWalletBalance(1, CryptoEnum.BTC.ToString(), 999M);
            procedures.UpdateUsersWalletBalance(1, CryptoEnum.ETH.ToString(), 999M);
            procedures.UpdateUsersWalletBalance(1, CryptoEnum.ADA.ToString(), 999M);
            procedures.UpdateUsersWalletBalance(1, CryptoEnum.ATOM.ToString(), 999M);
            procedures.UpdateUsersWalletBalance(1, CryptoEnum.DOGE.ToString(), 999M);
            var updatedBalances = procedures.GetUsersWalletBalances(1);

            // Assert
            Assert.NotEqual(previousBalances.BitcoinAmount, updatedBalances.BitcoinAmount);
            Assert.NotEqual(previousBalances.EtheriumAmount, updatedBalances.EtheriumAmount);
            Assert.NotEqual(previousBalances.CardanoAmount, updatedBalances.CardanoAmount);
            Assert.NotEqual(previousBalances.CosmosAmount, updatedBalances.CosmosAmount);
            Assert.NotEqual(previousBalances.DogecoinAmount, updatedBalances.DogecoinAmount);
        }

        /// <summary>
        /// Tests if GetSpecificWalletBalance returns correct balance amount.
        /// </summary>
        [Fact]
        public void GetSpecificWalletBalance_UserWithAllWallets_RequestedBalanceReturned()
        {
            // Arrange
            var procedures = new WalletProcedures(mockContext);

            // Act
            var usersBalances = procedures.GetUsersWalletBalances(1);
            var eurBalance = procedures.GetSpecificWalletBalance(1, FiatEnum.EUR.ToString());
            var btcBalance = procedures.GetSpecificWalletBalance(1, CryptoEnum.BTC.ToString());
            var ethBalance = procedures.GetSpecificWalletBalance(1, CryptoEnum.ETH.ToString());
            var adaBalance = procedures.GetSpecificWalletBalance(1, CryptoEnum.ADA.ToString());
            var atomBalance = procedures.GetSpecificWalletBalance(1, CryptoEnum.ATOM.ToString());
            var dogeBalance = procedures.GetSpecificWalletBalance(1, CryptoEnum.DOGE.ToString());

            // Assert
            Assert.Equal(usersBalances.EuroAmount, eurBalance);
            Assert.Equal(usersBalances.BitcoinAmount, btcBalance);
            Assert.Equal(usersBalances.EtheriumAmount, ethBalance);
            Assert.Equal(usersBalances.CardanoAmount, adaBalance);
            Assert.Equal(usersBalances.CosmosAmount, atomBalance);
            Assert.Equal(usersBalances.DogecoinAmount, dogeBalance);
        }

        /// <summary>
        /// Tests if GetUserWalletId returns correct walled id for requested user wallet.
        /// </summary>
        [Fact]
        public void GetUserWalletId_UserWithAllWallets_WalletIdReturned()
        {
            // Arrange
            var procedures = new WalletProcedures(mockContext);

            // Act
            var eurWalletId = procedures.GetUserWalletId(1, FiatEnum.EUR);

            // Assert
            Assert.Equal(1, eurWalletId);
        }
    }
}
