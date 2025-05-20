
using Microsoft.AspNetCore.SignalR;
using StockLiveDashboard.Contracts;
using StockLiveDashboard.WorkerService.Helpers;
using StockLiveDashboard.WorkerService.Hubs;

namespace StockLiveDashboard.WorkerService.WorkerEngines
{
    public class TradingEngine : IHostedService, IDisposable
    {
        private readonly ILogger<TradingEngine> _logger;
        private readonly IStocksUpdateHelper _stocksUpdateHelper;

        public TradingEngine(ILogger<TradingEngine> logger, IStocksUpdateHelper stocksUpdateHelper)
        {
            _logger = logger;
            _stocksUpdateHelper = stocksUpdateHelper;
        }

        public void Dispose()
        {
            _logger.LogInformation("Trading Engine Service started.");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start the engine");
            await _stocksUpdateHelper.CreateStocksSnapshot();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Engine stopped");
        }
    }
}
