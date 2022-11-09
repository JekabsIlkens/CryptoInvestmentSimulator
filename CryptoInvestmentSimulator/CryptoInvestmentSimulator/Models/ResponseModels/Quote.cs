using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.ResponseModels
{
    public class Quote
    {
        [JsonProperty("EUR")]
        public EUR? Euro { get; set; }
    }
}