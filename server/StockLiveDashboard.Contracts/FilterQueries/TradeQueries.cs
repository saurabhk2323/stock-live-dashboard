using StockLiveDashboard.Contracts.Entities;
using System.Linq.Expressions;
using LinqKit;

namespace StockLiveDashboard.Contracts.FilterQueries
{
    public static class TradeQueries
    {
        public static Expression<Func<Trade, bool>> GetFilteredTrades(string investorID, Trade? trade = null)
        {
            var predicate = PredicateBuilder.New<Trade>(t => t.InvestorID.ToString() == investorID);

            if (trade == null)
                return predicate;

            //if (!string.IsNullOrWhiteSpace(trade.InvestorUniqueName))
            //{
            //    predicate = predicate.And(t => t.InvestorUniqueName == trade.InvestorUniqueName);
            //}

            if (!string.IsNullOrWhiteSpace(trade.BuyOrSell))
            {
                predicate = predicate.And(t => t.BuyOrSell == trade.BuyOrSell);
            }

            //if (!string.IsNullOrEmpty(trade.ID.ToString()))
            //{
            //    predicate = predicate.And(t => t.ID.ToString() == trade.ID.ToString());
            //}

            if (!string.IsNullOrWhiteSpace(trade.StockID.ToString()))
            {
                predicate = predicate.And(t => t.StockID.ToString() == trade.StockID.ToString());
            }

            return predicate;
        }
    }
}
