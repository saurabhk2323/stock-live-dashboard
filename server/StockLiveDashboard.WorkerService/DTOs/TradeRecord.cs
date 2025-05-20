using StockLiveDashboard.Contracts.Entities;

namespace StockLiveDashboard.WorkerService.DTOs
{
    public record TradeRecord(
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
        public Trade ToTrade()
        {
            return new Trade()
            {
                InvestorID = Guid.Parse(investorID),
                InvestorUniqueName = investorUniqueName,
                BuyOrSell = buyOrSell,
                StockID = Guid.Parse(stockID),
                StockName = stockName,
                StockSymbol = stockSymbol,
                Price = price,
                Quantity = quantity
            };
        }
    }
}
