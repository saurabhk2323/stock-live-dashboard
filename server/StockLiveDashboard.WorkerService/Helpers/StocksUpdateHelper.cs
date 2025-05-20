using Microsoft.AspNetCore.SignalR;
using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.DTOs;
using StockLiveDashboard.Contracts.Entities;
using StockLiveDashboard.WorkerService.DTOs;
using StockLiveDashboard.WorkerService.Hubs;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace StockLiveDashboard.WorkerService.Helpers
{
    public class StocksUpdateHelper : IStocksUpdateHelper
    {
        private readonly IHubContext<StockHub> _hubContext;
        private readonly IStocksService _stocksService;

        private readonly ConcurrentDictionary<string, StockSnapshot> _stocksSnaphotsKVP;



        public StocksUpdateHelper(IHubContext<StockHub> hubContext, IStocksService stocksService)
        {
            _hubContext = hubContext;
            _stocksService = stocksService;
            _stocksSnaphotsKVP = new ConcurrentDictionary<string, StockSnapshot>();
        }

        /// <summary>
        /// During the initialization
        /// </summary>
        /// <returns></returns>
        public async Task CreateStocksSnapshot()
        {
            var stocks = await _stocksService.GetAllStocks();
            stocks.ForEach(stock =>
            {
                _stocksSnaphotsKVP.TryAdd(
                    stock.ID.ToString(),
                    new StockSnapshot
                    {
                        StockID = stock.ID.ToString(),
                        Sector = stock.Sector,
                        Price = stock.Price,
                        TotalShares = stock.TotalShares,
                        Investors = stock.InvestorsKind,
                        IsUpdated = false
                    });
            });
        }

        /// <summary>
        /// Whenever a successful trade takes place
        /// </summary>
        /// <param name="stockID"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="buyer"></param>
        /// <param name="seller"></param>
        public async Task UpdateStock(string stockID, decimal price, int quantity, InvestorKind buyer, InvestorKind seller)
        {
            if (_stocksSnaphotsKVP.TryGetValue(stockID, out StockSnapshot stockSnapshot))
            {
                stockSnapshot.Price = price;
                stockSnapshot.Investors?.ForEach(investor =>
                {
                    if (investor.Type == buyer.Type)
                        investor.TotalShare += quantity;

                    if (investor.Type == seller.Type)
                        investor.TotalShare -= quantity;

                    if (investor.TotalShare > 0)
                        investor.PercentageHolding = stockSnapshot.TotalShares / investor.TotalShare;
                });
                await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", stockSnapshot);
                stockSnapshot.IsUpdated = true;
            }
        }

        /// <summary>
        /// Periodically update the data into DB, only if there have been some changes in the response
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public async Task UpdateStockDetailsInDB(string stockID)
        {
            StockSnapshot stockSnapshot = new();
            _stocksSnaphotsKVP.TryGetValue(stockID, out stockSnapshot);
            if (stockSnapshot != null && stockSnapshot.IsUpdated)
            {
                StockResponse? stockResponse = await _stocksService.GetStockByID(
                    Guid.Parse(stockSnapshot.StockID),
                    stockSnapshot.Sector);
                stockResponse.Price = stockSnapshot.Price;
                stockResponse.InvestorsKind = stockSnapshot.Investors;
                StockUpdateRequest stockUpdateRequest = stockResponse.ToStockUpdateRequest();
                await _stocksService.UpdateStock(stockUpdateRequest);
            }
        }
    }
}
