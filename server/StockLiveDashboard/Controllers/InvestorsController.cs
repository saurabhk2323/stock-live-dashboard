using Microsoft.AspNetCore.Mvc;
using StockLiveDashboard.Contracts;
using StockLiveDashboard.Contracts.DTOs;

namespace StockLiveDashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestorsController : ControllerBase
    {
        private readonly IInvestorsService _investorService;

        public InvestorsController(IInvestorsService investorService)
        {
            _investorService = investorService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddInvestorAsync([FromBody] InvestorAddRequest investorAddRequest)
        {
            var response = await _investorService.AddInvestor(investorAddRequest);
            return new CreatedResult("investors container", response);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteInvestorAsync([FromQuery] Guid id, string countryID)
        {
            var response = await _investorService.DeleteInvestor(id, countryID);
            return Ok(response);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetInvestorAsync([FromQuery] Guid id, string countryID)
        {
            var response = await _investorService.GetInvestorByID(id, countryID);
            if (response == null)
                return new NotFoundResult();
            return Ok(response);
        }

        [HttpGet]
        [Route("get-by-username")]
        public async Task<IActionResult> GetInvestorByUsernameAsync([FromQuery] string username, string countryCode)
        {
            var response = await _investorService.GetInvestorByUsername(username, countryCode);
            if (response == null)
                return new NotFoundResult();
            return Ok(response);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllInvestorsAsync()
        {
            var response = await _investorService.GetAllInvestors();
            return Ok(response);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateInvestorAsync([FromBody] InvestorUpdateRequest investorUpdateRequest)
        {
            var response = await _investorService.UpdateInvestor(investorUpdateRequest);
            return Ok(response);
        }
    }
}
