using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.ResponseModels
{
    public class Root
    {
        [JsonProperty("status")]
        public Status? Status { get; set; }

        [JsonProperty("data")]
        public Data? Data { get; set; }
    }
}