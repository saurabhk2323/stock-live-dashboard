using Newtonsoft.Json;
using StockLiveDashboard.Contracts.Entities;
using StockLiveDashboard.Contracts.Enums;
using System;
using System.Collections.Generic;

namespace StockLiveDashboard.Contracts.DTOs
{
    public class StockAddRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string? Symbol { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string? Description { get; set; }

        /// <summary>
        /// CosmosDB: This property will be considered as partition key
        /// </summary>
        [JsonProperty(PropertyName = "sector")]
        public string? Sector { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "total_shares")]
        public int TotalShares { get; set; }

        [JsonProperty(PropertyName = "owner_investor_id")]
        public string? OwnerInvestorID { get; set; }

        [JsonProperty(PropertyName = "owner_type")]
        public string? OwnerType { get; set; }

        public Stock ToStock()
        {
            List<InvestorKind> kinds = new();
            foreach (var enumType in Enum.GetValues(typeof(InvestorKindEnum)))
            {
                InvestorKind investorKind = new InvestorKind()
                {
                    Type = enumType.ToString(),
                    TotalShare = OwnerType == enumType.ToString() ? TotalShares : 0,
                    PercentageHolding = OwnerType == enumType.ToString() ? 100 : 0
                };
                kinds.Add(investorKind);
            }
            return new Stock()
            {
                Name = Name,
                Symbol = Symbol,
                Description = Description,
                Sector = Sector,
                Price = Price,
                MarketCap = Price * TotalShares,
                TotalShares = TotalShares,
                InvestorsKind = kinds
            };
        }

        public Trade ToTrade()
        {
            return new Trade()
            {
                InvestorID = Guid.Parse(OwnerInvestorID),
                InvestorUniqueName = Symbol.ToLower(),
                BuyOrSell = BuyOrSell.buy.ToString(),
                StockName = Name,
                StockSymbol = Symbol,
                Price = Price,
                Quantity = 0,
                TotalOrderedQuantity = TotalShares,
                Status = Status.Success.ToString()
            };
        }
    }
}
