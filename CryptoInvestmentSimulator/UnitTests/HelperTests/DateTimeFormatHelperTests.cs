using CryptoInvestmentSimulator.Helpers;
using FluentAssertions;
using Xunit;

namespace UnitTests.HelperTests
{
    public class DateTimeFormatHelperTests
    {
        /// <summary>
        /// Tests if date time formatter correctly formats date.
        /// </summary>
        [Fact]
        public void ToDbFormatAsString_ValidDateParameter_ConvertedDateReturned()
        {
            // Arrange
            var validDate = new DateTime(2023, 7, 7);
            var expectedFormat = "2023-07-07 00:00:00";

            // Act
            var result = DateTimeFormatHelper.ToDbFormatAsString(validDate);

            // Assert
            var parsedResult = DateTime.Parse(result);

            Assert.NotNull(result);
            Assert.Equal(expectedFormat, result);
            Assert.Equal(validDate, parsedResult);
        }

        /// <summary>
        /// Tests if exeption is thrown when empty date is passed.
        /// </summary>
        [Fact]
        public void ToDbFormatAsString_NullDateParameter_ExceptionThrown()
        {
            // Arrange
            object invalidDate = null;

            // Act
            Action act = () => DateTimeFormatHelper.ToDbFormatAsString((DateTime)invalidDate);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
