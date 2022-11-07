using CryptoInvestmentSimulator.Helpers;
using FluentAssertions;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.HelperTests
{
    public class BooleanHelperTests
    {
        [Fact]
        public void StringToBool_ValidString_Success()
        {
            // Arrange
            var expectedValues = BooleanHelperMock.GetExpectedStrings();

            // Act
            var resultOne = BooleanHelper.StringToBool(expectedValues[0]);
            var resultTwo = BooleanHelper.StringToBool(expectedValues[1]);
            var resultThree = BooleanHelper.StringToBool(expectedValues[2]);

            // Assert
            resultOne.Should().Be(true);
            resultTwo.Should().Be(true);
            resultThree.Should().Be(true);
        }

        [Fact]
        public void StringToBool_InvalidString_Exception()
        {
            // Arrange
            var invalidValue = "hamster";

            // Act
            Action act = () => BooleanHelper.StringToBool(invalidValue);

            // Assert
            act.Should().ThrowExactly<ArgumentException>().WithMessage("Value cannot be converted!");
        }

        [Fact]
        public void IntToBool_ValidInteger_Success()
        {
            // Arrange
            var expectedValues = BooleanHelperMock.GetExpectedIntegers();

            // Act
            var resultOne = BooleanHelper.IntToBool(expectedValues[0]);
            var resultTwo = BooleanHelper.IntToBool(expectedValues[1]);
            var resultThree = BooleanHelper.IntToBool(expectedValues[2]);

            // Assert
            resultOne.Should().Be(true);
            resultTwo.Should().Be(false);
            resultThree.Should().Be(true);
        }
    }
}
