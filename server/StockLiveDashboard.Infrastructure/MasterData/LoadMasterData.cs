using Newtonsoft.Json;
using StockLiveDashboard.Contracts.Entities;

namespace StockLiveDashboard.Infrastructure.MasterData
{
    public static class LoadMasterData
    {
        public static List<Investor> GetInvestorList()
        {
            string json = @"
			[
				{
					""id"": ""06572d1f-cda9-4391-9cc7-ff53f6e2cf6c"",
					""name"": ""Reliance Industries Ltd."",
					""investor_unique_name"": ""relltd"",
					""investor_kind"": ""DII"",
					""country_code"": ""IN""
				},
				{
					""id"": ""d06c429a-e453-4a85-8d01-6e6da959c250"",
					""name"": ""Tech Innovations Inc."",
					""investor_unique_name"": ""tii"",
					""investor_kind"": ""DII"",
					""country_code"": ""IN""
				},
				{
					""id"": ""25b2e7b1-7888-4a72-b33e-1811ee62871e"",
					""name"": ""Global Healthcare Corp"",
					""investor_unique_name"": ""ghc"",
					""investor_kind"": ""DII"",
					""country_code"": ""IN""
				}
			]";
            return JsonConvert.DeserializeObject<List<Investor>>(json) ?? new List<Investor>();
        }

        public static List<Stock> GetStockList()
        {
            string json = @"
            [
                {
                    ""id"": ""376718cb-188c-4876-b228-e756a665cec6"",
                    ""name"": ""Reliance Industries Ltd."",
                    ""symbol"": ""RELLTD"",
                    ""description"": ""An industrial stock"",
                    ""sector"": ""Industry"",
                    ""price"": 1200,
                    ""market_cap"": 120100000,
                    ""total_shares"": 100000,
                    ""investors_kind"": [
                      {
                        ""type"": ""FII"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      },
                      {
                        ""type"": ""DII"",
                        ""total_share"": 100000,
                        ""percentage_holding"": 100
                      },
                      {
                        ""type"": ""Retailer"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      },
                      {
                        ""type"": ""Others"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      }
                    ]
                },
                {
                    ""id"": ""0e03e035-4c99-458d-85ef-0f0356dcee51"",
                    ""name"": ""Tech Innovations Inc."",
                    ""symbol"": ""TII"",
                    ""description"": ""A leading technology company focused on software and cloud services."",
                    ""sector"": ""Technology"",
                    ""price"": 155,
                    ""market_cap"": 778750000,
                    ""total_shares"": 5000000,
                    ""investors_kind"": [
                      {
                        ""type"": ""FII"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      },
                      {
                        ""type"": ""DII"",
                        ""total_share"": 5000000,
                        ""percentage_holding"": 100
                      },
                      {
                        ""type"": ""Retailer"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      },
                      {
                        ""type"": ""Others"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      }
                    ]
                },
                {
                    ""id"": ""294d5ea7-ee0b-4591-8aae-a63c633b361f"",
                    ""name"": ""Global Healthcare Corp"",
                    ""symbol"": ""GHC"",
                    ""description"": ""A multinational pharmaceutical and healthcare services provider."",
                    ""sector"": ""Healthcare"",
                    ""price"": 85,
                    ""market_cap"": 1278000000,
                    ""total_shares"": 15000000,
                    ""investors_kind"": [
                      {
                        ""type"": ""FII"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      },
                      {
                        ""type"": ""DII"",
                        ""total_share"": 15000000,
                        ""percentage_holding"": 100
                      },
                      {
                        ""type"": ""Retailer"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      },
                      {
                        ""type"": ""Others"",
                        ""total_share"": 0,
                        ""percentage_holding"": 0
                      }
                    ]
                }
            ]";

            return JsonConvert.DeserializeObject<List<Stock>>(json) ?? new List<Stock>();
        }

        public static List<Trade> GetTradeList()
        {
            string json = @"
            [
                {
                    ""id"": ""549e8ff3-cb90-48e3-8eb3-38f6bd507b38"",
                    ""investor_id"": ""06572d1f-cda9-4391-9cc7-ff53f6e2cf6c"",
                    ""investor_unique_name"": ""relltd"",
                    ""buy_or_sell"": ""buy"",
                    ""stock_id"": ""376718cb-188c-4876-b228-e756a665cec6"",
                    ""stock_name"": ""Reliance Industries Ltd."",
                    ""stock_symbol"": ""RELLTD"",
                    ""price"": 1200,
                    ""quantity"": 0,
                    ""total_ordered_quantity"": 100000,
                    ""status"": ""Success"",
                    ""trade_timestamp"": ""2025-05-20T10:50:57.1962301Z""
                },  
                {
                    ""id"": ""9ec10373-c305-440e-a107-80cc1a8905bb"",
                    ""investor_id"": ""d06c429a-e453-4a85-8d01-6e6da959c250"",
                    ""investor_unique_name"": ""tii"",
                    ""buy_or_sell"": ""buy"",
                    ""stock_id"": ""0e03e035-4c99-458d-85ef-0f0356dcee51"",
                    ""stock_name"": ""Tech Innovations Inc."",
                    ""stock_symbol"": ""TII"",
                    ""price"": 155,
                    ""quantity"": 0,
                    ""total_ordered_quantity"": 5000000,
                    ""status"": ""Success"",
                    ""trade_timestamp"": ""2025-05-20T10:51:06.4061147Z""
                },  
                {
                    ""id"": ""B621D29E-5274-4A89-BB6B-2D6097BFF115"",
                    ""investor_id"": ""25b2e7b1-7888-4a72-b33e-1811ee62871e"",
                    ""investor_unique_name"": ""GHC"",
                    ""buy_or_sell"": ""BUY"",
                    ""stock_id"": ""376718cb-188c-4876-b228-e756a665cec6"",
                    ""stock_name"": ""Global Healthcare Corp"",
                    ""stock_symbol"": ""GHC"",
                    ""price"": 85,
                    ""quantity"": 0,
                    ""total_ordered_quantity"": 15000000,
                    ""status"": ""Success"",
                    ""trade_timestamp"": ""2025-05-20T10:51:06.4061147Z""
                }
            ]";

            return JsonConvert.DeserializeObject<List<Trade>>(json) ?? new List<Trade>();
        }
    }
}
