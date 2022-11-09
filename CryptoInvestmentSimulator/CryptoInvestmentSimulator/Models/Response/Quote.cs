using Newtonsoft.Json;

namespace CryptoInvestmentSimulator.Models.Response
{
    public class Quote
    {
        [JsonProperty("EUR")]
        public EUR? Euro { get; set; }
    }
}