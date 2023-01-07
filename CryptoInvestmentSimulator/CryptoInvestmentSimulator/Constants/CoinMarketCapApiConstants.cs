namespace CryptoInvestmentSimulator.Constants
{
    public static class CoinMarketCapApiConstants
    {
        // CoinMarketCap API configuration and access constants.
        public static readonly string AccessKey = "b54bcf4d-1bca-4e8e-9a24-22ff2c3d462c";

        // Sandbox endpoints for testing.
        public static readonly string LatestQuotesTest = "https://sandbox-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";

        // Real endpoints for production.
        public static readonly string LatestQuotes = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";
    }
}
