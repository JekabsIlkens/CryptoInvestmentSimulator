using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using FluentAssertions;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class DatabaseContextTests
    {
        [Fact]
        public void GetConnection_ConnectionStringValid_Success()
        {
            // Arrange
            var databaseContext = new DatabaseContext(DatabaseConstants.Access);

            // Act
            var result = databaseContext.GetConnection();

            // Assert
            result.Should().NotBe(null);
        }

        [Fact]
        public void GetConnection_ConnectionStringNull_Exception()
        {
            // Arrange
            var databaseContext = new DatabaseContext("");

            // Act
            Action act = () => databaseContext.GetConnection();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
