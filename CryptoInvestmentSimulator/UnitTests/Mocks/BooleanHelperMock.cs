namespace UnitTests.Mocks
{
    public class BooleanHelperMock
    {
        /// <summary>
        /// Returns list of accepted scenarios for StringToBool method
        /// </summary>
        /// <returns>{"true", "True", "TRUE"}</returns>
        public static List<string> GetExpectedStrings()
        {
            return new List<string>(){"true", "True", "TRUE"};
        }

        /// <summary>
        /// Returns list of accepted scenarios for IntToBool method
        /// </summary>
        /// <returns>{ Negative-Int, 0, Positive-Int }</returns>
        public static List<int> GetExpectedIntegers()
        {
            var random = new Random();

            return new List<int>() { random.Next(-100, 0), 0, random.Next(1, 101) };
        }
    }
}
