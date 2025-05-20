using StockLiveDashboard.WorkerService.DTOs;
using System.Threading.Channels;

namespace StockLiveDashboard.WorkerService.Services
{
    public class TradeQueue : ITradeQueue
    {
        private readonly Channel<TradeRecord> _channel = Channel.CreateUnbounded<TradeRecord>();
        public ChannelWriter<TradeRecord> Writer => _channel.Writer;
        public ChannelReader<TradeRecord> Reader => _channel.Reader;
    }
}
