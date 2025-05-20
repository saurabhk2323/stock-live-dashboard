using StockLiveDashboard.Contracts.Entities;

namespace StockLiveDashboard.WorkerService.Helpers
{
    public interface IStocksUpdateHelper
    {
        Task CreateStocksSnapshot();

        Task UpdateStock(string stockID, decimal price, int quantity, InvestorKind buyer, InvestorKind seller);

        Task UpdateStockDetailsInDB(string stockID);
    }
}
