namespace CryptoInvestmentSimulator.Helpers
{
    public class FloatingPointHelper
    {
        /// <summary>
        /// Converts floating point number to 2 places with round up.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// Decimal value rounded to 2 points.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static decimal FloatingPointToTwo(double? value)
        {
            if (value != null) return Math.Round((decimal)value, 2);
            else throw new ArgumentNullException("Received null!"); ;
        }

        /// <summary>
        /// Converts floating point number to 6 places with round up.
        /// </summary>
        /// <param name="value">Value to round.</param>
        /// <returns>
        /// Decimal value rounded to 6 points.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static decimal FloatingPointToSix(double? value)
        {
            if (value != null) return Math.Round((decimal)value, 6);
            else throw new ArgumentNullException("Received null!"); ;
        }
    }
}
