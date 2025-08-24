using ExchangeRate.Application.Interface.AwesomeAPI;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.API.Controllers.AwesomeAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwesomeAPIController : Controller
    {
        private readonly IAwesomeAPIService _awesomeAPIService;

        public AwesomeAPIController(IAwesomeAPIService awesomeAPIService)
        {
            _awesomeAPIService = awesomeAPIService;
        }

        [HttpGet("{coin}")]
        public async Task<IActionResult> GetCurrency(string coin)
        {
            try
            {
                var result  = await _awesomeAPIService.GetCurrencyAsync(coin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
