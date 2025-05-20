using StockLiveDashboard.WorkerService.DTOs;
using System.Threading.Channels;

namespace StockLiveDashboard.WorkerService.Services
{
    public interface ITradeQueue
    {
        ChannelWriter<TradeRecord> Writer { get; }
        ChannelReader<TradeRecord> Reader { get; }
    }
}
