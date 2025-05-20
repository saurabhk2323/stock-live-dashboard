using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.Entities;
using StockLiveDashboard.Contracts.Enums;
using StockLiveDashboard.Services.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StockLiveDashboard.Services
{
    public class TradesService : ITradesService
    {
        private readonly CosmosDBService _db;

        public TradesService(CosmosDBService db)
        {
            _db = db;
        }

        public async Task<Trade> CreateTradeOrder(Trade trade)
        {
            return await _db.CreateItem(
                _db.GetContainer(ContainerTypes.trades),
                trade,
                trade.InvestorID.ToString()
                );
        }

        public async Task<bool> DeleteTradeOrder(Trade trade)
        {
            return await _db.DeleteItem<Trade>(
                _db.GetContainer(ContainerTypes.trades),
                trade.ID.ToString(),
                trade.InvestorID.ToString());
        }

        public async Task<IEnumerable<Trade>> GetAllTradeOrders(Expression<Func<Trade, bool>> predicate, string investorID)
        {
            return await _db.GetItems<Trade>(
                _db.GetContainer(ContainerTypes.trades),
                predicate,
                investorID);
        }

        public async Task<Trade?> GetTradeByID(Guid tradeID, Guid investorID)
        {
            return await _db.GetItem<Trade>(
                _db.GetContainer(ContainerTypes.trades),
                tradeID.ToString(),
                investorID.ToString()
                );
        }

        public async Task<Trade> UpdateTradeOrder(Trade trade)
        {
            return await _db.UpdateItem(
                _db.GetContainer(ContainerTypes.trades),
                trade,
                trade.InvestorID.ToString());
        }
    }
}
