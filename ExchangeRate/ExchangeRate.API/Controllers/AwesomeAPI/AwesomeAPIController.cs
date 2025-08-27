using ExchangeRate.Application.Interface.AwesomeAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.API.Controllers.AwesomeAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AwesomeAPIController : Controller
    {
        private readonly IAwesomeAPIService _awesomeAPIService;

        public AwesomeAPIController(IAwesomeAPIService awesomeAPIService)
        {
            _awesomeAPIService = awesomeAPIService;
        }

        [HttpGet("{currency}")]
        public async Task<IActionResult> GetCurrency(string currency)
        {
            try
            {
                var result  = await _awesomeAPIService.GetLastCurrencyAsync(currency);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
