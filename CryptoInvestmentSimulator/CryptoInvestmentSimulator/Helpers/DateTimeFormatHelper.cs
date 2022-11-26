using CryptoInvestmentSimulator.Constants;

namespace CryptoInvestmentSimulator.Helpers
{
    public class DateTimeFormatHelper
    {
        /// <summary>
        /// Converts C# DateTime to Database supported DateTime (ISO8601).
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>DateTime in DB format as string</returns>
        /// <example>2022-11-12 23:59:59</example>
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
