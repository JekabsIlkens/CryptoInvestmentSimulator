using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.ResponseModels
{
    public class Data
    {
        [JsonProperty("BTC")]
        public BTC? Bitcoin { get; set; }

        [JsonProperty("ETH")]
        public ETH? Etherium { get; set; }

        [JsonProperty("ATOM")]
        public ATOM? Cosmos { get; set; }

        [JsonProperty("ADA")]
        public ADA? Cardano { get; set; }

        [JsonProperty("DOGE")]
        public DOGE? Dogecoin { get; set; }
    }
}