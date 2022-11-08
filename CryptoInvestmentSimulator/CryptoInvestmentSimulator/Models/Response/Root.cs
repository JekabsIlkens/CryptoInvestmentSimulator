using Newtonsoft.Json;

namespace CryptocurrencyInvestmentSimulator.Models.ResponseModels
{
    public class Root
    {
        [JsonProperty("status")]
        public Status? Status { get; set; }

        [JsonProperty("data")]
        public Data? Data { get; set; }
    }
}