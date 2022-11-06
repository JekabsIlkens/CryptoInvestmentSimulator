namespace CryptoInvestmentSimulator.Helpers
{
    public class BooleanHelper
    {
        /// <summary>
        /// Converts passed string to a boolean.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Bool</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool StringToBool(string value)
        {
            if (value == "true" || value == "True" || value == "TRUE") return true;
            else if (value == "false" || value == "False" || value == "FALSE") return false;
            else throw new ArgumentException("Value cannot be converted!");
        }

        /// <summary>
        /// Converts passed integer to a boolean.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Bool</returns>
        public static bool IntToBool(int value)
        {
            if (value != 0) return true;
            else return false;
        }
    }
}
