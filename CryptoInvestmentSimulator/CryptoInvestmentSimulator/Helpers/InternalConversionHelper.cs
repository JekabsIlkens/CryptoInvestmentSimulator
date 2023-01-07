using CryptoInvestmentSimulator.Enums;

namespace CryptoInvestmentSimulator.Helpers
{
    public static class InternalConversionHelper
    {
        /// <summary>
        /// Converts crypto symbol string to coresponding crypto enum.
        /// </summary>
        /// <param name="symbolString">Symbol string.</param>
        /// <returns>
        /// Matching crypto enum.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static CryptoEnum StringToCryptoEnum(string symbolString)
        {
            return symbolString switch
            {
                "BTC" => CryptoEnum.BTC,
                "ETH" => CryptoEnum.ETH,
                "ADA" => CryptoEnum.ADA,
                "ATOM" => CryptoEnum.ATOM,
                "DOGE" => CryptoEnum.DOGE,
                _ => throw new ArgumentException(symbolString)
            };
        }

        /// <summary>
        /// Converts integer to coresponding crypto enum.
        /// </summary>
        /// <param name="number">Number to convert.</param>
        /// <returns>
        /// Matching crypto enum.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static CryptoEnum IntToCryptoEnum(int number)
        {
            return number switch
            {
                1 => CryptoEnum.BTC,
                2 => CryptoEnum.ETH,
                3 => CryptoEnum.ADA,
                4 => CryptoEnum.ATOM,
                5 => CryptoEnum.DOGE,
                _ => throw new ArgumentException(nameof(number))
            };
        }

        /// <summary>
        /// Converts users selected time zone to actual hour change value.
        /// </summary>
        /// <param name="timeZone">Selected time zone.</param>
        /// <returns>
        /// Hour change value, positive, zero or negative.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int TimeZoneStringToChangeValue(string timeZone)
        {
            return timeZone switch
            {
                "GMT-12:00" => -12,
                "GMT-11:00" => -11,
                "GMT-10:00" => -10,
                "GMT-09:00" => -9,
                "GMT-08:00" => -8,
                "GMT-07:00" => -7,
                "GMT-06:00" => -6,
                "GMT-05:00" => -5,
                "GMT-04:00" => -4,
                "GMT-03:00" => -3,
                "GMT-02:00" => -2,
                "GMT-01:00" => -1,
                "GMT+00:00" => 0,
                "GMT+01:00" => 1,
                "GMT+02:00" => 2,
                "GMT+03:00" => 3,
                "GMT+04:00" => 4,
                "GMT+05:00" => 5,
                "GMT+06:00" => 6,
                "GMT+07:00" => 7,
                "GMT+08:00" => 8,
                "GMT+09:00" => 9,
                "GMT+10:00" => 10,
                "GMT+11:00" => 11,
                "GMT+12:00" => 12,
                _ => throw new ArgumentOutOfRangeException(nameof(timeZone)),
            };
        }
    }
}
