using CryptoInvestmentSimulator.Helpers;
using FluentAssertions;
using Xunit;

namespace UnitTests.HelperTests
{
    public class ConfidenceHelperTests
    {
        /// <summary>
        /// Tests if random alphanumeric key is returned successfuly with valid length.
        /// </summary>
        [Fact]
        public void GetRandomKey_ValidLengthParameter_KeyReturned()
        {
            // Arrange
            var requestedLength = 10;

            // Act
            var result = ConfidenceHelper.GetRandomKey(requestedLength);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(requestedLength, result.Length);
        }

        /// <summary>
        /// Tests if exception is thrown if provided length parameter is invalid.
        /// </summary>
        [Fact]
        public void GetRandomKey_InvalidLengthParameter_ExceptionThrown()
        {
            // Arrange
            var requestedLength = -1;

            // Act
            Action act = () => ConfidenceHelper.GetRandomKey(requestedLength);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
