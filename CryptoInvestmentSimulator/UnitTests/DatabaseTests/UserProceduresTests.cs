using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using CryptoInvestmentSimulator.Enums;
using CryptoInvestmentSimulator.Models.ViewModels;
using FluentAssertions;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class UserProceduresTests
    {
        /// <summary>
        /// Tests if all <see cref="UserProcedures"/> methods return exception
        /// when provided parameters are null or empty.
        /// </summary>
        [Fact]
        public void AllMethods_InvalidParameters_Exception()
        {
            // Arrange
            var userProcedures = new UserProcedures(new DatabaseContext(DatabaseConstants.Access));
            var mockUser = ModelMock.GetInvalidUserModel();

            // Act
            Action act1 = () => userProcedures.GetUserDetails(mockUser.Email);
            Action act2 = () => userProcedures.InsertNewUser(mockUser);
            Action act3 = () => userProcedures.UpdateUsername(mockUser.Email, mockUser.Username);
            Action act4 = () => userProcedures.UpdateAvatar(mockUser.Email, mockUser.Avatar);
            Action act5 = () => userProcedures.UpdateTimeZone(mockUser.Email, 1);
            Action act6 = () => userProcedures.UpdateVerification(mockUser.Email);
            Action act7 = () => userProcedures.DoesUserExist(mockUser.Email);

            // Assert
            act1.Should().ThrowExactly<ArgumentNullException>();
            act2.Should().ThrowExactly<ArgumentNullException>();
            act3.Should().ThrowExactly<ArgumentNullException>();
            act4.Should().ThrowExactly<ArgumentNullException>();
            act5.Should().ThrowExactly<ArgumentNullException>();
            act6.Should().ThrowExactly<ArgumentNullException>();
            act7.Should().ThrowExactly<ArgumentNullException>();
        }

        /// <summary>
        /// Tests if filled user model gets returned when calling
        /// GetUserDetails on an existing user in database.
        /// </summary>
        [Fact]
        public void GetUserDetails_UserExistsInDb_FilledUserModel()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();

            // Act
            var result = procedures.GetUserDetails(mockUser.Email);
            testServer.ShutDown();

            // Assert
            Assert.Equal(mockUser.Username, result.Username);
        }

        /// <summary>
        /// Tests if empty user model gets returned when calling
        /// GetUserDetails on a non-existant user in database.
        /// </summary>
        [Fact]
        public void GetUserDetails_UserNotInDb_EmptyUserModel()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            mockUser.Email = "missing.user@yahoo.com";

            // Act
            var result = procedures.GetUserDetails(mockUser.Email);
            testServer.ShutDown();

            // Assert
            Assert.Null(result.Username);
        }

        /// <summary>
        /// Tests if new user gets inserted into database only when
        /// user doesn't already exist in database.
        /// </summary>
        [Fact]
        public void InsertNewUser_UserDoesNotExist_UserInserted()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var newUser = new UserModel()
            {
                Id = 2,
                Username = "mock-username",
                Email = "mock-email",
                Avatar = "mock-url",
                Verified = 0,
                TimeZone = "mock-timezone"
            };

            procedures.InsertNewUser(newUser);
            var query = $"SELECT * FROM user WHERE email = '{newUser.Email}'";

            // Act
            var result = DatabaseMock.QueryHasRows(mockConnection, query);
            testServer.ShutDown();

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
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            mockUser.Username = "Spongebob";

            procedures.UpdateUsername(mockUser.Email, mockUser.Username);
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseMock.GetUsernameValue(mockConnection, query);
            testServer.ShutDown();

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
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            mockUser.Avatar = "new-url";

            procedures.UpdateAvatar(mockUser.Email, mockUser.Avatar);
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseMock.GetAvatarValue(mockConnection, query);
            testServer.ShutDown();

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
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            mockUser.TimeZone = "GMT+02:00";

            procedures.UpdateTimeZone(mockUser.Email, 15);
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseMock.GetTimeZoneValue(mockConnection, query);
            testServer.ShutDown();

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
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            var oldStatus = mockUser.Verified;

            procedures.UpdateVerification(mockUser.Email);
            var query = $"SELECT * FROM user WHERE email = '{mockUser.Email}'";

            // Act
            var result = DatabaseMock.GetVerificationValue(mockConnection, query);
            testServer.ShutDown();

            // Assert
            Assert.NotEqual(oldStatus.ToString(), result);
        }

        /// <summary>
        /// Tests if existance check returns true for existing user.
        /// </summary>
        [Fact]
        public void DoesUserExist_ExistingUser_ReturnsTrue()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            procedures.InsertNewUser(mockUser);

            // Act
            var result = procedures.DoesUserExist(mockUser.Email);
            testServer.ShutDown();

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests if existance check returns false for nonexistant user.
        /// </summary>
        [Fact]
        public void DoesUserExist_NonexistantUser_ReturnsFalse()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            mockUser.Email = "notexist@gmail.com";

            // Act
            var result = procedures.DoesUserExist(mockUser.Email);
            testServer.ShutDown();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests if wallet creation for a new user works as expected
        /// and initial balance for each wallet is correct.
        /// </summary>
        [Fact]
        public void CreateWalletsForUser_ExistingUser_WalletsCreated()
        {
            // Arrange
            var testServer = DatabaseMock.CreateDatabase();
            var mockConnection = testServer.GetConnectionString("test");
            var mockContext = new DatabaseContext(mockConnection);

            var procedures = new UserProcedures(mockContext);
            var mockUser = ModelMock.GetValidUserModel();
            mockUser.Id = 2;
            mockUser.Email = "walletTest@mail.com";

            // Act
            procedures.InsertNewUser(mockUser);
            procedures.CreateWalletsForUser(mockUser.Email);

            var eurQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{FiatEnum.EUR}'";
            var eurWalletResult = DatabaseMock.GetWalletBalance(mockConnection, eurQuery);

            var btcQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.BTC}'";
            var btcWalletResult = DatabaseMock.GetWalletBalance(mockConnection, btcQuery);

            var ethQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.ETH}'";
            var ethWalletResult = DatabaseMock.GetWalletBalance(mockConnection, ethQuery);

            var adaQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.ADA}'";
            var adaWalletResult = DatabaseMock.GetWalletBalance(mockConnection, adaQuery);

            var atomQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.ATOM}'";
            var atomWalletResult = DatabaseMock.GetWalletBalance(mockConnection, atomQuery);

            var dogeQuery = $"SELECT * FROM wallet WHERE user_id = {mockUser.Id} AND symbol = '{CryptoEnum.DOGE}'";
            var dogeWalletResult = DatabaseMock.GetWalletBalance(mockConnection, dogeQuery);

            testServer.ShutDown();

            // Assert
            Assert.Equal(5000M, eurWalletResult);
            Assert.Equal(0M, btcWalletResult);
            Assert.Equal(0M, ethWalletResult);
            Assert.Equal(0M, adaWalletResult);
            Assert.Equal(0M, atomWalletResult);
            Assert.Equal(0M, dogeWalletResult);
        }
    }
}
