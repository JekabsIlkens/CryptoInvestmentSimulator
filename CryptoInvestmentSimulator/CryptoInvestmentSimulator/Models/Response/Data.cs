using Newtonsoft.Json;

namespace CryptocurrencyInvestmentSimulator.Models.ResponseModels
{
    public class Data
    {
        [JsonProperty("BTC")]
        public BTC? Bitcoin { get; set; }
    }
}