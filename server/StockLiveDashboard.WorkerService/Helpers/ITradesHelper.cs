using StockLiveDashboard.WorkerService.DTOs;

namespace StockLiveDashboard.WorkerService.Helpers
{
    public interface ITradesHelper
    {
        void ExecuteTrade(TradeRecord trade);
    }
}
