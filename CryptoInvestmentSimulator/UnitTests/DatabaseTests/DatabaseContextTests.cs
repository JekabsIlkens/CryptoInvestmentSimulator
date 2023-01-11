using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;
using FluentAssertions;
using Xunit;

namespace UnitTests.DatabaseTests
{
    public class DatabaseContextTests
    {
        /// <summary>
        /// Tests if passing an empty access to context retrieval throws exception.
        /// </summary>
        [Fact]
        public void GetConnection_ConnectionStringNull_Exception()
        {
            // Arrange
            var databaseContext = new DatabaseContext(string.Empty);

            // Act
            Action act = () => databaseContext.GetConnection();

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Tests if database context for database is successfuly retrieved.
        /// </summary>
        [Fact]
        public void GetConnection_ConnectionStringValid_Success()
        {
            // Arrange
            var databaseContext = new DatabaseContext(DatabaseConstants.AzureAccess);

            // Act
            var result = databaseContext.GetConnection();

            // Assert
            result.Should().NotBe(null);
        }
    }
}
