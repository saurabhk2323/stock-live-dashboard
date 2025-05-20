using Newtonsoft.Json;
using StockLiveDashboard.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockLiveDashboard.Contracts.DTOs
{
    public class InvestorAddRequest
    {
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

        public Investor ToInvestor()
        {
            return new Investor()
            {
                Name = Name,
                InvestorUniqueName = InvestorUniqueName,
                InvestorKind = InvestorKind,
                CountryCode = CountryCode
            };
        }
    }
}
