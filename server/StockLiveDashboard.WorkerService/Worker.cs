using StockLiveDashboard.WorkerService.Helpers;
using StockLiveDashboard.WorkerService.Services;

namespace StockLiveDashboard.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITradeQueue _queue;
        private readonly ITradesHelper _tradesHelper;
        private readonly IStocksUpdateHelper _stocksUpdateHelper;

        public Worker(ILogger<Worker> logger, ITradeQueue queue, ITradesHelper tradesHelper, IStocksUpdateHelper stocksUpdateHelper)
        {
            _logger = logger;
            _queue = queue;
            _tradesHelper = tradesHelper;
            _stocksUpdateHelper = stocksUpdateHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (var trade in _queue.Reader.ReadAllAsync(stoppingToken))
                {
                    _logger.LogInformation(
                        "Processing order: {StockID} - {Quantity} - {BuyOrSell}",
                        trade.stockID,
                        trade.quantity,
                        trade.buyOrSell
                        );
                    _tradesHelper.ExecuteTrade(trade);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Worker");
            }
        }
    }
}
