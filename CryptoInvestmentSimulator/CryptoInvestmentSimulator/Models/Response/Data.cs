using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.Response
{
    public class Data
    {
        [JsonProperty("BTC")]
        public BTC? Bitcoin { get; set; }
    }
}