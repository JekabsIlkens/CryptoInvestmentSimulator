using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.Response
{
    public class BTC
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("slug")]
        public string? Slug { get; set; }

        [JsonProperty("num_market_pairs")]
        public int? NumMarketPairs { get; set; }

        [JsonProperty("date_added")]
        public DateTime? DateAdded { get; set; }

        [JsonProperty("tags")]
        public List<string>? Tags { get; set; }

        [JsonProperty("max_supply")]
        public int? MaxSupply { get; set; }

        [JsonProperty("circulating_supply")]
        public int? CirculatingSupply { get; set; }

        [JsonProperty("total_supply")]
        public int? TotalSupply { get; set; }

        [JsonProperty("is_active")]
        public int? IsActive { get; set; }

        [JsonProperty("platform")]
        public object? Platform { get; set; }

        [JsonProperty("cmc_rank")]
        public int? CoinMarketCapRank { get; set; }

        [JsonProperty("is_fiat")]
        public int? IsFiat { get; set; }

        [JsonProperty("self_reported_circulating_supply")]
        public object? SelfReportedCirculatingSupply { get; set; }

        [JsonProperty("self_reported_market_cap")]
        public object? SelfReportedMarketCap { get; set; }

        [JsonProperty("tvl_ratio")]
        public object? TotalValueLockedRatio { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [JsonProperty("quote")]
        public Quote? Quote { get; set; }
    }
}