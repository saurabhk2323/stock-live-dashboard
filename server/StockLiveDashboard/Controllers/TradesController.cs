using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.FilterQueries;

namespace StockLiveDashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly ITradesService _tradesService;

        public TradesController(ITradesService tradesService)
        {
            _tradesService = tradesService;
        }


        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllOrders([FromQuery] string investorID)
        {
            var response = await _tradesService.GetAllTradeOrders(TradeQueries.GetFilteredTrades(investorID), investorID);
            return Ok(response);
        }
    }
}
