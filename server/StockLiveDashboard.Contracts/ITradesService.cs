using StockLiveDashboard.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace StockLiveDashboard.Contracts
{
    public interface ITradesService
    {
        Task<Trade> CreateTradeOrder(Trade trade);

        Task<bool> DeleteTradeOrder(Trade trade);

        Task<IEnumerable<Trade>> GetAllTradeOrders(Expression<Func<Trade, bool>> predicate, string investorID);
        
        Task<Trade?> GetTradeByID(Guid tradeID, Guid stockID);
        
        Task<Trade> UpdateTradeOrder(Trade trade);
    }
}
