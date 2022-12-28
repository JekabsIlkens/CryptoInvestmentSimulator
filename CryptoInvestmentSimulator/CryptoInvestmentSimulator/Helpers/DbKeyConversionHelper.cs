using System;

namespace CryptoInvestmentSimulator.Helpers
{
    public static class DbKeyConversionHelper
    {
        /// <summary>
        /// Converts string type time zone from into coresponding database key.
        /// </summary>
        /// <param name="timeZone">String time zone</param>
        /// <returns>String time zone converted to its db key</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int TimeZoneToDbKey(string timeZone)
        {
            return timeZone switch
            {
                "GMT-12:00" => 1,
                "GMT-11:00" => 2,
                "GMT-10:00" => 3,
                "GMT-09:00" => 4,
                "GMT-08:00" => 5,
                "GMT-07:00" => 6,
                "GMT-06:00" => 7,
                "GMT-05:00" => 8,
                "GMT-04:00" => 9,
                "GMT-03:00" => 10,
                "GMT-02:00" => 11,
                "GMT-01:00" => 12,
                "GMT+00:00" => 13,
                "GMT+01:00" => 14,
                "GMT+02:00" => 15,
                "GMT+03:00" => 16,
                "GMT+04:00" => 17,
                "GMT+05:00" => 18,
                "GMT+06:00" => 19,
                "GMT+07:00" => 20,
                "GMT+08:00" => 21,
                "GMT+09:00" => 22,
                "GMT+10:00" => 23,
                "GMT+11:00" => 24,
                "GMT+12:00" => 25,
                _ => throw new ArgumentOutOfRangeException(nameof(timeZone)),
            };
        }

        /// <summary>
        /// Converts the key type time zone from database into coresponding string value.
        /// </summary>
        /// <param name="timeZone">Time zone key</param>
        /// <returns>Key converted to string time zone</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string TimeZoneKeyToString(int timeZone)
        {
            return timeZone switch
            {
                1 => "GMT-12:00",
                2 => "GMT-11:00",
                3 => "GMT-10:00",
                4 => "GMT-09:00",
                5 => "GMT-08:00",
                6 => "GMT-07:00",
                7 => "GMT-06:00",
                8 => "GMT-05:00",
                9 => "GMT-04:00",
                10 => "GMT-03:00",
                11 => "GMT-02:00",
                12 => "GMT-01:00",
                13 => "GMT+00:00",
                14 => "GMT+01:00",
                15 => "GMT+02:00",
                16 => "GMT+03:00",
                17 => "GMT+04:00",
                18 => "GMT+05:00",
                19 => "GMT+06:00",
                20 => "GMT+07:00",
                21 => "GMT+08:00",
                22 => "GMT+09:00",
                23 => "GMT+10:00",
                24 => "GMT+11:00",
                25 => "GMT+12:00",
                _ => throw new ArgumentOutOfRangeException(nameof(timeZone)),
            };
        }

        /// <summary>
        /// Converts crypto symbol to coresponding database key.
        /// </summary>
        /// <param name="crypto">Crypto symbol</param>
        /// <returns>Database key for symbol</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int CryptoSymbolToDbKey(string crypto)
        {
            return crypto switch
            {
                "BTC" => 1,
                "ETH" => 2,
                "ADA" => 3,
                "ATOM" => 4,
                "DOGE" => 5,
                _ => throw new ArgumentOutOfRangeException(nameof(crypto)),
            };
        }

        /// <summary>
        /// Converts database key to coresponding crypto symbol.
        /// </summary>
        /// <param name="crypto">Crypto database key</param>
        /// <returns>Crypto symbol</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string CryptoKeyToSymbol(int crypto)
        {
            return crypto switch
            {
                1 => "BTC",
                2 => "ETH",
                3 => "ADA",
                4 => "ATOM",
                5 => "DOGE",
                _ => throw new ArgumentOutOfRangeException(nameof(crypto)),
            };
        }

        /// <summary>
        /// Converts fiat symbol to coresponding database key.
        /// </summary>
        /// <param name="fiat">Fiat symbol</param>
        /// <returns>Database key for symbol</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int FiatSymbolToDbKey(string fiat)
        {
            return fiat switch
            {
                "EUR" => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(fiat)),
            };
        }

        /// <summary>
        /// Converts database key to coresponding fiat symbol.
        /// </summary>
        /// <param name="fiat">Fiat database key</param>
        /// <returns>Fiat symbol</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string FiatKeyToSymbol(int fiat)
        {
            return fiat switch
            {
                1 => "EUR",
                _ => throw new ArgumentOutOfRangeException(nameof(fiat)),
            };
        }

        public static int LeverageStringToDbKey(string leverageMultiplier)
        {
            return leverageMultiplier switch
            {
                "1x" => 0,
                "2x" => 1,
                "5x" => 2,
                "10x" => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(leverageMultiplier)),
            };
        }
    }
}
