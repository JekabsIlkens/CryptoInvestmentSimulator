namespace CryptoInvestmentSimulator.Models.ViewModels
{
    public class MarketDataModel
    {
        public string CryptoSymbol { get; set; }
        public string FiatSymbol { get; set; }
        public DateTime CollectionDateTime { get; set; }
        public decimal FiatPricePerUnit { get; set; }
        public decimal PercentChange24h { get; set; }
        public decimal PercentChange7d { get; set; }
    }
}
