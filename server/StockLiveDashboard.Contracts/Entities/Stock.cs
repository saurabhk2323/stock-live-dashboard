using Newtonsoft.Json;
using StockLiveDashboard.Contracts.DTOs;
using System;
using System.Collections.Generic;

namespace StockLiveDashboard.Contracts.Entities
{
    public class Stock
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }

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

        [JsonProperty(PropertyName = "market_cap")]
        public decimal MarketCap { get; set; }

        [JsonProperty(PropertyName = "total_shares")]
        public int TotalShares { get; set; }

        [JsonProperty(PropertyName = "investors_kind")]
        public List<InvestorKind>? InvestorsKind { get; set; }
    }

    public static class StockExtensions
    {
        public static StockResponse ToStockResponse(this Stock stock)
        {
            return new StockResponse()
            {
                ID = stock.ID,
                Name = stock.Name,
                Symbol = stock.Symbol,
                Description = stock.Description,
                Sector = stock.Sector,
                Price = stock.Price,
                MarketCap = stock.MarketCap,
                TotalShares = stock.TotalShares,
                InvestorsKind = stock.InvestorsKind,
            };
        }
    }
}
