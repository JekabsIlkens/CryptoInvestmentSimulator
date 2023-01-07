using CryptoInvestmentSimulator.Enums;

namespace CryptoInvestmentSimulator.Helpers
{
    public static class InternalConversionHelper
    {
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

        public static CryptoEnum IntToCryptoEnum(int symbolKey)
        {
            return symbolKey switch
            {
                1 => CryptoEnum.BTC,
                2 => CryptoEnum.ETH,
                3 => CryptoEnum.ADA,
                4 => CryptoEnum.ATOM,
                5 => CryptoEnum.DOGE,
                _ => throw new ArgumentException(nameof(symbolKey))
            };
        }
    }
}
