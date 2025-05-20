using StockLiveDashboard.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockLiveDashboard.Contracts
{
    public interface IStocksService
    {
        Task<StockResponse> AddStock(StockAddRequest stockAddRequest);

        Task<StockResponse> UpdateStock(StockUpdateRequest stockUpdateRequest);

        Task<bool> DeleteStock(Guid stockID, string sector);

        Task<StockResponse?> GetStockByID(Guid stockID, string sector);

        Task<List<StockResponse>> GetAllStocks();

        Task<StockInfoByInvestor> GetStockInfoByInvestor(string investorID, string stockID);
    }
}
