using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.Entities;
using StockLiveDashboard.Contracts.Enums;
using StockLiveDashboard.Contracts.FilterQueries;
using StockLiveDashboard.WorkerService.DTOs;
using System.Collections.Concurrent;

namespace StockLiveDashboard.WorkerService.Helpers
{
    /// <summary>
    /// Helper class for managing trade execution and order books.
    /// </summary>
    public class TradesHelper : ITradesHelper
    {
        private readonly ITradesService _tradesService;
        private readonly IStocksUpdateHelper _stocksUpdateHelper;

        /// <summary>
        /// In-memory buyer order book: StockID -> (Price -> OrderedTrades)
        /// </summary>
        private readonly ConcurrentDictionary<string, SortedDictionary<decimal, OrderedTrades>> _stockBuyers;

        /// <summary>
        /// In-memory seller order book: StockID -> (Price -> OrderedTrades)
        /// </summary>
        private readonly ConcurrentDictionary<string, SortedDictionary<decimal, OrderedTrades>> _stockSellers;

        public TradesHelper(ITradesService tradesService, IStocksUpdateHelper stocksUpdateHelper)
        {
            _tradesService = tradesService;
            _stocksUpdateHelper = stocksUpdateHelper;
            _stockBuyers = new ConcurrentDictionary<string, SortedDictionary<decimal, OrderedTrades>>();
            _stockSellers = new ConcurrentDictionary<string, SortedDictionary<decimal, OrderedTrades>>();
        }

        /// <summary>
        /// Executes a trade: matches with counter orders if possible, otherwise adds to order book.
        /// </summary>
        /// <param name="tradeRecord"></param>
        public async void ExecuteTrade(TradeRecord tradeRecord)
        {
            Trade trade = tradeRecord.ToTrade();
            trade.TradeTimestamp = DateTime.UtcNow;

            await SaveOrUpdateTradeOrderAsync(trade);

            bool isTradePossible = false;
            decimal lastTradingPrice = -1;

            // Determine if this is a sell or buy order and process accordingly
            if (tradeRecord.buyOrSell.Equals(BuyOrSell.sell.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                isTradePossible = TryExecuteSell(trade, ref lastTradingPrice);

                if (!isTradePossible)
                {
                    AddToSellerBook(trade);
                }
            }
            else
            {
                isTradePossible = TryExecuteBuy(trade, ref lastTradingPrice);

                if (!isTradePossible)
                {
                    AddToBuyerBook(trade);
                }
            }
        }

        /// <summary>
        /// Saves a new trade order or updates an existing one in the database.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        private async Task SaveOrUpdateTradeOrderAsync(Trade trade)
        {
            var tradeResponse = await _tradesService.GetAllTradeOrders(
                TradeQueries.GetFilteredTrades(trade.InvestorID.ToString(), trade),
                trade.InvestorID.ToString());

            if (tradeResponse.Any() && tradeResponse.FirstOrDefault().Price == trade.Price)
            {
                var tradeDetails = tradeResponse.First();
                tradeDetails.Quantity += trade.Quantity;
                trade.ID = tradeDetails.ID;
                trade.TotalOrderedQuantity += trade.Quantity;
                _ = _tradesService.UpdateTradeOrder(tradeDetails);
            }
            else
            {
                trade.TotalOrderedQuantity = trade.Quantity;
                trade.ID = Guid.NewGuid();
                trade.Status = Status.Pending.ToString();
                _ = _tradesService.CreateTradeOrder(trade);
            }
        }

        /// <summary>
        /// Attempts to match and execute a sell order with existing buy orders.
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="lastTradingPrice"></param>
        /// <returns></returns>
        private bool TryExecuteSell(Trade trade, ref decimal lastTradingPrice)
        {
            if (_stockBuyers.TryGetValue(trade.StockID.ToString(), out var buyerTrades))
            {
                if (IsTradePossible(trade, buyerTrades, BuyOrSell.sell))
                {
                    lastTradingPrice = trade.Price;
                    _ = MatchAndExecuteTrade(trade, buyerTrades, lastTradingPrice, isSell: true);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Attempts to match and execute a buy order with existing sell orders.
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="lastTradingPrice"></param>
        /// <returns></returns>
        private bool TryExecuteBuy(Trade trade, ref decimal lastTradingPrice)
        {
            if (_stockSellers.TryGetValue(trade.StockID.ToString(), out var sellerTrades))
            {
                if (IsTradePossible(trade, sellerTrades, BuyOrSell.buy))
                {
                    _ = MatchAndExecuteTrade(trade, sellerTrades, lastTradingPrice, isSell: false);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a sell order to the seller order book if not matched.
        /// </summary>
        /// <param name="trade"></param>
        private void AddToSellerBook(Trade trade)
        {
            if (_stockSellers.TryGetValue(trade.StockID.ToString(), out var tradesDict))
            {
                if (tradesDict.TryGetValue(trade.Price, out var orderedTrades))
                {
                    orderedTrades.Quantity += trade.Quantity;
                    var existingTrade = orderedTrades.tradeList.FindIndex(t => t.InvestorID == trade.InvestorID);
                    if (existingTrade != -1)
                    {
                        orderedTrades.tradeList[existingTrade].Quantity += trade.Quantity;
                    }
                    else
                    {
                        orderedTrades.tradeList.Add(trade);
                    }
                }
                else
                {
                    tradesDict.TryAdd(trade.Price, new OrderedTrades { Quantity = trade.Quantity, tradeList = new List<Trade> { trade } });
                }
            }
            else
            {
                var tradeList = new List<Trade> { trade };
                var sortedTrades = new SortedDictionary<decimal, OrderedTrades>();
                sortedTrades.TryAdd(trade.Price, new OrderedTrades { Quantity = trade.Quantity, tradeList = tradeList });
                _stockSellers.TryAdd(trade.StockID.ToString(), sortedTrades);
            }
        }

        /// <summary>
        /// Adds a buy order to the buyer order book if not matched.
        /// </summary>
        /// <param name="trade"></param>
        private void AddToBuyerBook(Trade trade)
        {
            if (_stockBuyers.TryGetValue(trade.StockID.ToString(), out var tradesDict))
            {
                if (tradesDict.TryGetValue(trade.Price, out var orderedTrades))
                {
                    orderedTrades.Quantity += trade.Quantity;
                    var existingTrade = orderedTrades.tradeList.FindIndex(t => t.InvestorID == trade.InvestorID);
                    if (existingTrade != -1)
                    {
                        orderedTrades.tradeList[existingTrade].Quantity += trade.Quantity;
                    }
                    else
                    {
                        orderedTrades.tradeList.Add(trade);
                    }
                }
                else
                {
                    tradesDict.TryAdd(trade.Price, new OrderedTrades { Quantity = trade.Quantity, tradeList = new List<Trade> { trade } });
                }
            }
            else
            {
                var tradeList = new List<Trade> { trade };
                // Buyers book is sorted in descending order of price
                var sortedTrades = new SortedDictionary<decimal, OrderedTrades>(Comparer<decimal>.Create((x, y) => y.CompareTo(x)));
                sortedTrades.TryAdd(trade.Price, new OrderedTrades { Quantity = trade.Quantity, tradeList = tradeList });
                _stockBuyers.TryAdd(trade.StockID.ToString(), sortedTrades);
            }
        }

        /// <summary>
        /// Matches and executes trades between the given trade and counter orders.
        /// Updates quantities, statuses, and removes filled orders.
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="counterTrades"></param>
        /// <param name="lastTradingPrice"></param>
        /// <param name="isSell"></param>
        /// <returns></returns>
        private async Task MatchAndExecuteTrade(Trade trade, SortedDictionary<decimal, OrderedTrades> counterTrades, decimal lastTradingPrice, bool isSell)
        {
            var removeTrades = new List<decimal>();

            foreach (var (price, orderedTrades) in counterTrades)
            {
                int matchedQuantity = 0;
                var removeTradeList = new List<Trade>();

                foreach (var orderedTrade in orderedTrades.tradeList)
                {
                    // Skip self-matching
                    if (orderedTrade.InvestorID == trade.InvestorID) continue;

                    lastTradingPrice = isSell ? trade.Price : orderedTrade.Price;
                    int tradeQty = Math.Min(trade.Quantity, orderedTrade.Quantity);

                    // Update stock with matched quantity
                    await _stocksUpdateHelper.UpdateStock(
                        trade.StockID.ToString(),
                        lastTradingPrice,
                        tradeQty,
                        new InvestorKind { Type = InvestorKindEnum.Retailer.ToString() },
                        new InvestorKind { Type = InvestorKindEnum.Retailer.ToString() });

                    orderedTrade.Quantity -= tradeQty;
                    trade.Quantity -= tradeQty;
                    matchedQuantity += tradeQty;

                    if (orderedTrade.Quantity == 0)
                    {
                        removeTradeList.Add(orderedTrade);
                        orderedTrade.Status = Status.Success.ToString();
                    }
                    else
                    {
                        orderedTrade.Status = Status.Partial.ToString();
                    }

                    await _tradesService.UpdateTradeOrder(orderedTrade);

                    // Exit if the incoming trade is fully matched
                    if (trade.Quantity == 0)
                    {
                        trade.Status = Status.Success.ToString();
                        await _tradesService.UpdateTradeOrder(trade);
                        break;
                    }
                }

                // Remove fully matched trades from the order book
                orderedTrades.tradeList = orderedTrades.tradeList.Except(removeTradeList).ToList();
                orderedTrades.Quantity -= matchedQuantity;
                if (orderedTrades.Quantity == 0)
                    removeTrades.Add(price);
                if (trade.Quantity == 0)
                    break;
            }

            // Remove price levels with no remaining orders
            foreach (var price in removeTrades)
            {
                counterTrades.Remove(price);
            }
        }

        /// <summary>
        /// Checks if a trade can be matched with existing counter orders based on price and quantity.
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="placedTrades"></param>
        /// <param name="buyOrSell"></param>
        /// <returns></returns>
        private bool IsTradePossible(Trade trade, SortedDictionary<decimal, OrderedTrades> placedTrades, BuyOrSell buyOrSell)
        {
            // check and compare the price
            var filteredTradesByPrice = placedTrades
                .Where(
                    t => buyOrSell == BuyOrSell.buy ? t.Key <= trade.Price : t.Key >= trade.Price);

            int availableQuantity = 0;
            foreach (var (price, orderedTrades) in filteredTradesByPrice)
            {
                foreach (var orderedTrade in orderedTrades.tradeList)
                {
                    // exclude same investors placed order
                    if (orderedTrade.InvestorID != trade.InvestorID)
                    {
                        availableQuantity += orderedTrade.Quantity;
                    }
                }
            }

            return availableQuantity > 0;
        }
    }
}
