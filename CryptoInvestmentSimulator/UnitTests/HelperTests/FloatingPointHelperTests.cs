using CryptoInvestmentSimulator.Helpers;
using Xunit;

namespace UnitTests.HelperTests
{
    public class FloatingPointHelperTests
    {
        [Fact]
        public void FloatingPointToTwo_EightFloatingPoints_ReturnsTwoPoints()
        {
            // Arrange
            var numberToConvert = 10.12345678;

            // Act
            var result = FloatingPointHelper.FloatingPointToTwo(numberToConvert);
            var expectedResult = 10.12M;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FloatingPointToSix_EightFloatingPoints_ReturnsSixPoints()
        {
            // Arrange
            var numberToConvert = 10.12345678;

            // Act
            var result = FloatingPointHelper.FloatingPointToSix(numberToConvert);
            var expectedResult = 10.123457M;

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
