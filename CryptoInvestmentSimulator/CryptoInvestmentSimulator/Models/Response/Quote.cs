using Newtonsoft.Json;

namespace CryptocurrencyInvestmentSimulator.Models.ResponseModels
{
    public class Quote
    {
        [JsonProperty("EUR")]
        public EUR? Euro { get; set; }
    }
}