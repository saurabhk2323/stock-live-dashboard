using StockLiveDashboard.Contracts.Entities;

namespace StockLiveDashboard.WorkerService.DTOs
{
    public class OrderedTrades
    {
        public int Quantity { get; set; }
        public List<Trade> tradeList { get; set; }
    }
}
