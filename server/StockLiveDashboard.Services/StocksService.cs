using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.DTOs;
using StockLiveDashboard.Contracts.Entities;
using StockLiveDashboard.Services.DatabaseServices;
using StockLiveDashboard.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace StockLiveDashboard.Services
{
    public class StocksService : IStocksService
    {
        private readonly CosmosDBService _db;
        private readonly ITradesService _tradesService;

        public StocksService(CosmosDBService db, ITradesService tradesService)
        {
            _db = db;
            _tradesService = tradesService;
        }

        public async Task<StockResponse> AddStock(StockAddRequest stockAddRequest)
        {
            Stock stock = stockAddRequest.ToStock();
            stock.ID = Guid.NewGuid();
            var response = await _db.CreateItem(
                _db.GetContainer(ContainerTypes.stocks),
                stock,
                stock.Sector
                );

            Trade trade = stockAddRequest.ToTrade();
            trade.ID = Guid.NewGuid();
            trade.StockID = stock.ID;
            trade.TradeTimestamp = DateTime.UtcNow;
            await _tradesService.CreateTradeOrder(trade);

            return response.ToStockResponse();
        }

        public async Task<bool> DeleteStock(Guid stockID, string sector)
        {
            return await _db.DeleteItem<Stock>(
                _db.GetContainer(ContainerTypes.stocks),
                stockID.ToString(),
                sector);
        }

        public async Task<List<StockResponse>> GetAllStocks()
        {
            Expression<Func<Stock, bool>> predicate = p => true;
            var response = await _db.GetItems<Stock>(
                _db.GetContainer(ContainerTypes.stocks),
                predicate
                );
            List<StockResponse> stockResponses = new();
            response.ForEach(s => stockResponses.Add(s.ToStockResponse()));
            return stockResponses;
        }

        public async Task<StockResponse?> GetStockByID(Guid stockID, string sector)
        {
            Stock? stock = await _db.GetItem<Stock>(
                _db.GetContainer(ContainerTypes.stocks),
                stockID.ToString(),
                sector
                );
            if (stock == null)
                return null;
            return stock.ToStockResponse();
        }

        public async Task<StockInfoByInvestor> GetStockInfoByInvestor(string investorID, string stockID)
        {
            Expression<Func<Trade, bool>> predicate = p =>
                p.InvestorID.ToString() == investorID &&
                p.StockID.ToString() == stockID;
            var tradeListResponse = await _tradesService.GetAllTradeOrders(predicate, investorID);
            StockInfoByInvestor stockInfoByInvestor = new();
            foreach (var trade in tradeListResponse)
            {
                if (trade.Status == Status.Success.ToString())
                {
                    stockInfoByInvestor.Holding += trade.TotalOrderedQuantity;
                    continue;
                }

                if (trade.Status == Status.Pending.ToString() || trade.Status == Status.Partial.ToString())
                {
                    if (trade.BuyOrSell == BuyOrSell.buy.ToString())
                        stockInfoByInvestor.Buyers += trade.Quantity;
                    else
                        stockInfoByInvestor.Sellers += trade.Quantity;
                }
            }
            return stockInfoByInvestor;
        }

        public async Task<StockResponse> UpdateStock(StockUpdateRequest updateStockRequest)
        {
            Stock stock = updateStockRequest.ToStock();
            var response = await _db.UpdateItem(
                    _db.GetContainer(ContainerTypes.stocks),
                    stock,
                    stock.Sector
                    );
            StockResponse stockResponse = response.ToStockResponse();
            return stockResponse;
        }
    }
}
