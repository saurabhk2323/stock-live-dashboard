using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StockLiveDashboard.Contracts.Entities
{
    public class Trade
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }

        /// <summary>
        /// CosmosDB: This property will be considered as partition key
        /// </summary>
        [JsonProperty(PropertyName = "investor_id")]
        public Guid InvestorID { get; set; }

        [JsonProperty(PropertyName = "investor_unique_name")]
        public string? InvestorUniqueName { get; set; }

        [JsonProperty(PropertyName = "buy_or_sell")]
        public string? BuyOrSell { get; set; }

        [JsonProperty(PropertyName = "stock_id")]
        public Guid StockID { get; set; }

        [JsonProperty(PropertyName = "stock_name")]
        public string? StockName { get; set; }

        [JsonProperty(PropertyName = "stock_symbol")]
        public string? StockSymbol { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "total_ordered_quantity")]
        public int TotalOrderedQuantity { get; set; }

        [JsonProperty(PropertyName ="status")]
        public string? Status { get; set; }

        [JsonProperty(PropertyName = "trade_timestamp")]
        public DateTime? TradeTimestamp { get; set; }
    }
}