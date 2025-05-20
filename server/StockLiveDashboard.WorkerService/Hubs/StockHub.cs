using Microsoft.AspNetCore.SignalR;
using StockLiveDashboard.WorkerService.DTOs;
using StockLiveDashboard.WorkerService.Services;


namespace StockLiveDashboard.WorkerService.Hubs
{
    public class StockHub : Hub
    {
        private readonly ITradeQueue _tradeQueue;

        public StockHub(ITradeQueue tradeQueue)
        {
            _tradeQueue = tradeQueue;
        }

        public async Task BroadCastStocks()
        {
            await Clients.All.SendAsync("ReceiveStockUpdate", new object());
        }

        public Task PlaceOrder(
            string investorID,
            string investorUniqueName,
            string buyOrSell,
            string stockID,
            string stockName,
            string stockSymbol,
            decimal price,
            int quantity
            )
        {
            var trade = new TradeRecord(investorID, investorUniqueName, buyOrSell, stockID, stockName, stockSymbol, price, quantity);
            return _tradeQueue.Writer.WriteAsync(trade).AsTask();
        }
    }
}
