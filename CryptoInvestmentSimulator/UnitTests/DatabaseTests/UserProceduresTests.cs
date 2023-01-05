using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Helpers;
using FluentAssertions;
using MySql.Server;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class UserProceduresTests
    {
        private static readonly MySqlServer mockServer = DatabaseInstanceMock.CreateMockDatabase();
        private static readonly string mockConnection = mockServer.GetConnectionString("mockdb");
        private static readonly DatabaseContext mockContext = new DatabaseContext(mockConnection);

        /// <summary>
        /// Tests if filled user model gets returned when calling
        /// GetUserDetails on an existing user in database.
        /// </summary>
        [Fact]
        public void GetUserDetails_UserExistsInDb_FilledUserModel()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidVerifiedUserModel();

            // Act
            var result = procedures.GetUserDetails(mockUser.Email);
            mockServer.ShutDown();

            // Assert
            Assert.Equal(mockUser.Id, result.Id);
            Assert.Equal(mockUser.Email, result.Email);
            Assert.Equal(mockUser.Verified, result.Verified);
            Assert.Equal(mockUser.Username, result.Username);
            Assert.Equal(mockUser.Avatar, result.Avatar);
            Assert.Equal(mockUser.TimeZone, result.TimeZone);
        }

        /// <summary>
        /// Tests if empty user model gets returned when calling
        /// GetUserDetails on a non-existant user in database.
        /// </summary>
        [Fact]
        public void GetUserDetails_UserNotInDb_EmptyUserModel()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidVerifiedUserModel();
            mockUser.Email = "unknown@mail.com";

            // Act
            var result = procedures.GetUserDetails(mockUser.Email);
            mockServer.ShutDown();

            // Assert
            Assert.Null(result.Email);
            Assert.Null(result.Username);
            Assert.Null(result.Avatar);
            Assert.Null(result.TimeZone);
        }

        /// <summary>
        /// Tests if new user gets inserted into database only when
        /// user doesn't already exist in database.
        /// </summary>
        [Fact]
        public void InsertNewUser_UserDoesNotExist_UserInserted()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var newUser = ModelMock.GetValidVerifiedUserModel();
            newUser.Id = 2;
            newUser.Email = "new@mail.com";

            var query = $"SELECT * FROM user WHERE email = '{newUser.Email}'";

            // Act
            procedures.InsertNewUser(newUser);
            var result = DatabaseInstanceMock.QueryHasRows(mockConnection, query);

            mockServer.ShutDown();

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests if username field gets updated for existing user.
        /// </summary>
        [Fact]
        public void UpdateUsername_ValidParameters_UsernameUpdated()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidVerifiedUserModel();
            mockUser.Username = "Pete";

            procedures.UpdateUsername(mockUser.Id, mockUser.Username);
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseInstanceMock.GetUsernameValue(mockConnection, query);
            mockServer.ShutDown();

            // Assert
            Assert.Equal(mockUser.Username, result);
        }

        /// <summary>
        /// Tests if avatar field gets updated for existing user.
        /// </summary>
        [Fact]
        public void UpdateAvatar_ValidParameters_AvatarUpdated()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidVerifiedUserModel();
            mockUser.Avatar = "new-avatar";

            procedures.UpdateAvatar(mockUser.Id, mockUser.Avatar);
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseInstanceMock.GetAvatarValue(mockConnection, query);
            mockServer.ShutDown();

            // Assert
            Assert.Equal(mockUser.Avatar, result);
        }

        /// <summary>
        /// Tests if timezone field gets updated for existing user.
        /// </summary>
        [Fact]
        public void UpdateTimeZone_ValidParameters_TimeZoneUpdated()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidVerifiedUserModel();
            mockUser.TimeZone = "GMT-08:00";

            procedures.UpdateTimeZone(mockUser.Id, DbKeyConversionHelper.TimeZoneToDbKey(mockUser.TimeZone));
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseInstanceMock.GetTimeZoneValue(mockConnection, query);
            mockServer.ShutDown();

            // Assert
            Assert.Equal(mockUser.TimeZone, result);
        }

        /// <summary>
        /// Tests if verification field gets updated for existing user.
        /// </summary>
        [Fact]
        public void UpdateVerification_ValidParameters_VerificationUpdated()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidVerifiedUserModel();
            mockUser.Verified = 0;
            var oldStatus = mockUser.Verified;

            procedures.UpdateVerification(mockUser.Id);
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseInstanceMock.GetVerificationValue(mockConnection, query);
            mockServer.ShutDown();

            // Assert
            Assert.NotEqual(oldStatus.ToString(), result);
        }

        /// <summary>
        /// Tests if wallet creation for a new user works as expected
        /// and initial balance for each wallet is correct.
        /// </summary>
        [Fact]
        public void CreateWalletsForUser_ExistingUser_WalletsCreated()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidVerifiedUserModel();
            mockUser.Id = 2;
            mockUser.Email = "walletTest@mail.com";

            // Act
            procedures.InsertNewUser(mockUser);
            procedures.CreateWalletsForUser(mockUser.Id);

            var eurQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{FiatEnum.EUR}'";
            var eurWalletResult = DatabaseInstanceMock.GetWalletBalance(mockConnection, eurQuery);

            var btcQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.BTC}'";
            var btcWalletResult = DatabaseInstanceMock.GetWalletBalance(mockConnection, btcQuery);

            var ethQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.ETH}'";
            var ethWalletResult = DatabaseInstanceMock.GetWalletBalance(mockConnection, ethQuery);

            var adaQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.ADA}'";
            var adaWalletResult = DatabaseInstanceMock.GetWalletBalance(mockConnection, adaQuery);

            var atomQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.ATOM}'";
            var atomWalletResult = DatabaseInstanceMock.GetWalletBalance(mockConnection, atomQuery);

            var dogeQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.DOGE}'";
            var dogeWalletResult = DatabaseInstanceMock.GetWalletBalance(mockConnection, dogeQuery);

            mockServer.ShutDown();

            // Assert
            Assert.Equal(5000M, eurWalletResult);
            Assert.Equal(0M, btcWalletResult);
            Assert.Equal(0M, ethWalletResult);
            Assert.Equal(0M, adaWalletResult);
            Assert.Equal(0M, atomWalletResult);
            Assert.Equal(0M, dogeWalletResult);
        }

        /// <summary>
        /// Tests if GetAllVerifiedUserIds correctly returns only verified user ids.
        /// </summary>
        [Fact]
        public void GetAllVerifiedUserIds_OneVerifiedOneUnverifiedInDb_ReturnsOneItem()
        {
            // Arrange
            var procedures = new UserProcedures(mockContext);

            var verifiedUser = ModelMock.GetValidVerifiedUserModel();

            var unverifiedUser = ModelMock.GetValidVerifiedUserModel();
            unverifiedUser.Id = 2;
            unverifiedUser.Email = "new@mail.com";
            unverifiedUser.Verified = 0;

            procedures.InsertNewUser(verifiedUser);
            procedures.InsertNewUser(unverifiedUser);

            // Act
            var result = procedures.GetAllVerifiedUserIds();
            mockServer.ShutDown();

            // Assert
            Assert.Single(result);
        }
    }
}
