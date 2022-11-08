using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using FluentAssertions;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class UserProceduresTests
    {
        [Fact]
        public void InsertNewUser_ValidUserModel_Success()
        {
            // Arrange
            var userProcedures = new UserProcedures(new DatabaseContext(DatabaseConstants.Access));
            var mockUserModel = ModelMock.GetValidUserModel();

            // Act
            Action act = () => userProcedures.InsertNewUser(mockUserModel);

            // Assert
            act.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void InsertNewUser_InvalidUserModel_Exception()
        {
            // Arrange
            var userProcedures = new UserProcedures(new DatabaseContext(DatabaseConstants.Access));
            var mockUserModel = ModelMock.GetValidUserModel();
            mockUserModel.EmailAddress = null;

            // Act
            Action act = () => userProcedures.InsertNewUser(mockUserModel);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
