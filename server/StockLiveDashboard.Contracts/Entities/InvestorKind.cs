using Newtonsoft.Json;
using System.Text.Json;

namespace StockLiveDashboard.Contracts.Entities
{
    public class InvestorKind
    {
        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }

        [JsonProperty(PropertyName = "total_share")]
        public int TotalShare { get; set; }

        [JsonProperty(PropertyName = "percentage_holding")]
        public decimal PercentageHolding { get; set; }
    }
}
