using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.Response
{
    public class Root
    {
        [JsonProperty("status")]
        public Status? Status { get; set; }

        [JsonProperty("data")]
        public Data? Data { get; set; }
    }
}