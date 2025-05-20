using Microsoft.AspNetCore.Mvc;
using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.DTOs;

namespace StockLiveDashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStocksService _stocksService;

        public StocksController(IStocksService stocksService)
        {
            _stocksService = stocksService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddStockAsync([FromBody] StockAddRequest stockAddRequest)
        {
            var response = await _stocksService.AddStock(stockAddRequest);
            return new CreatedResult("stocks container", response);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteStockAsync([FromQuery] Guid id, string sector)
        {
            var response = await _stocksService.DeleteStock(id, sector);
            return Ok(response);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetStockAsync([FromQuery] Guid id, string sector)
        {
            var response = await _stocksService.GetStockByID(id, sector);
            if (response == null)
                return new NotFoundResult();
            return Ok(response);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllStocksAsync()
        {
            var response = await _stocksService.GetAllStocks();
            return Ok(response);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateStockAsync([FromBody] StockUpdateRequest stockUpdateRequest)
        {
            var response = await _stocksService.UpdateStock(stockUpdateRequest);
            return Ok(response);
        }

        [HttpGet]
        [Route("get-stock-details-by-investor")]
        public async Task<IActionResult> GetStockInfoByInvestorAsync([FromQuery] string investorID, string stockID)
        {
            var response = await _stocksService.GetStockInfoByInvestor(investorID, stockID);
            return Ok(response);
        }
    }
}
