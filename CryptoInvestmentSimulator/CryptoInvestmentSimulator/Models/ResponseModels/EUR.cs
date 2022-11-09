using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.ResponseModels
{
    public class EUR
    {
        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("volume_24h")]
        public double? Volume24h { get; set; }

        [JsonProperty("volume_change_24h")]
        public double? VolumeChange24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public double? PercentChange1h { get; set; }

        [JsonProperty("percent_change_24h")]
        public double? PercentChange24h { get; set; }

        [JsonProperty("percent_change_7d")]
        public double? PercentChange7d { get; set; }

        [JsonProperty("percent_change_30d")]
        public double? PercentChange30d { get; set; }

        [JsonProperty("percent_change_60d")]
        public double? PercentChange60d { get; set; }

        [JsonProperty("percent_change_90d")]
        public double? PercentChange90d { get; set; }

        [JsonProperty("market_cap")]
        public double? MarketCap { get; set; }

        [JsonProperty("market_cap_dominance")]
        public double? MarketCapDominance { get; set; }

        [JsonProperty("fully_diluted_market_cap")]
        public double? FullyDilutedMarketCap { get; set; }

        [JsonProperty("tvl")]
        public object? TotalValueLocked { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }
    }
}