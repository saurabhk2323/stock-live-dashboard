using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockLiveDashboard.Contracts.DTOs
{
    public class InvestorResponse
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "investor_unique_name")]
        public string? InvestorUniqueName { get; set; }

        [JsonProperty(PropertyName = "investor_kind")]
        public string? InvestorKind { get; set; }

        /// <summary>
        /// CosmosDB: This property will be considered as partition key
        /// </summary>
        [JsonProperty(PropertyName = "country_code")]
        public string? CountryCode { get; set; }
    }
}
