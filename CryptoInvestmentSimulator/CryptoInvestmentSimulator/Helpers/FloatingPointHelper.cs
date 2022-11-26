namespace CryptoInvestmentSimulator.Helpers
{
    public class FloatingPointHelper
    {
        /// <summary>
        /// Converts floating point number to int without round up
        /// </summary>
        /// <param name="value"></param>
        /// <returns>15.859822 -> 15</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int FloatingPointToZero(double? value)
        {
            if (value != null) return (int)(value * 100);
            else throw new ArgumentNullException("Received null!");
        }

        /// <summary>
        /// Converts floating point number to 2 places with round up
        /// </summary>
        /// <param name="value"></param>
        /// <returns>15.859822 -> 15.86</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static decimal FloatingPointToTwo(double? value)
        {
            if (value != null) return Math.Round((decimal)value, 2);
            else throw new ArgumentNullException("Received null!"); ;
        }

        /// <summary>
        /// Converts floating point number to 4 places with round up
        /// </summary>
        /// <param name="value"></param>
        /// <returns>15.859822 -> 15.8598</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static decimal FloatingPointToFour(double? value)
        {
            if (value != null) return Math.Round((decimal)value, 4);
            else throw new ArgumentNullException("Received null!"); ;
        }
    }
}
