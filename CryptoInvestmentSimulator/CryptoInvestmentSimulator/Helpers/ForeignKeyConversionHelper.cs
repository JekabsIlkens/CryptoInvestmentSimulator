namespace CryptoInvestmentSimulator.Helpers
{
    public static class ForeignKeyConversionHelper
    {
        /// <summary>
        /// Converts the string type time zone from dropdown
        /// into coresponding database id.
        /// </summary>
        /// <param name="timeZone">String time zone form dropdown</param>
        /// <returns>String time zone converted to its db id</returns>
        /// <exception cref="ArgumentException"></exception>
        public static int TimeZoneStringToFK(string timeZone)
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
                _ => throw new ArgumentException(nameof(timeZone)),
            };
        }
    }
}
