namespace CryptoInvestmentSimulator.Constants
{
    public static class DatabaseConstants
    {
        // Database configuration and access constants.
        public static readonly string Access = "server=localhost;port=7770;database=cisdb;user=cisadmin;password=CISadminPASS";

        // Formatting constants.
        public static readonly string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        // Table column constants.
        public static readonly string LeverageRatioColumns = "multiplier";
        public static readonly string CryptoSymbolColumns = "symbol";
        public static readonly string FiatSymbolColumns = "symbol";
        public static readonly string TimeZoneColumns = "change";
        public static readonly string StatusColumns = "name";

        public static readonly string WalletColumns = "symbol, balance, user_id";
        public static readonly string UserColumns = "email, verified, username, avatar, zone_id";
        public static readonly string MarketDataColumns = "date_time, unit_value, daily_change, weekly_change, crypto_id, fiat_id";
        public static readonly string PositionColumns = "date_time, fiat_amount, crypto_amount, margin, ratio_id, status_id, wallet_id, data_id";

    }
}
