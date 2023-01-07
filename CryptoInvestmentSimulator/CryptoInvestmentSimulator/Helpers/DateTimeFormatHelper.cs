using CryptoInvestmentSimulator.Constants;

namespace CryptoInvestmentSimulator.Helpers
{
    public class DateTimeFormatHelper
    {
        /// <summary>
        /// Converts C# DateTime to Database supported DateTime (ISO8601).
        /// </summary>
        /// <param name="dateTime">C# date time.</param>
        /// <returns>
        /// DateTime in DB format as string.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToDbFormatAsString(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString()))
            {
                throw new ArgumentNullException($"Received {nameof(dateTime)} is null or empty!");
            }

            return dateTime.ToString(DatabaseConstants.DateTimeFormat);
        }
    }
}
