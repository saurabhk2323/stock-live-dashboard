using Newtonsoft.Json;
using StockLiveDashboard.Contracts.DTOs;
using System;
using System.Collections.Generic;

namespace StockLiveDashboard.Contracts.Entities
{
    public class Investor
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

        public InvestorResponse ToInvestorResponse()
        {
            return new InvestorResponse()
            {
                ID = ID,
                Name = Name,
                InvestorUniqueName = InvestorUniqueName,
                InvestorKind = InvestorKind,
                CountryCode = CountryCode
            };
        }
    }
}
