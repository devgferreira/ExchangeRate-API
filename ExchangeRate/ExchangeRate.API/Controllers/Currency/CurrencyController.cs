using ExchangeRate.Application.DTO.Currency.Request;
using ExchangeRate.Application.Interface.Currency;
using ExchangeRate.Domain.Entity.Currency.Request;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.API.Controllers.Currency
{
    [ApiController]
    [Route("api/[controller]")]

    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        [HttpGet("CalculatePriceBidVariationOnTheDay")]
        public async Task<IActionResult> CurrencyCalculatePriceBidVariationOnTheDay([FromQuery] CurrencyRequest request)
        {
            try
            {
                var result = await _currencyService.CurrencyCalculatePriceBidVariationOnTheDay(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CurrencyCalculateAverageSpreadOnTheDay")]
        public async Task<IActionResult> CurrencyCalculateAverageSpreadOnTheDay([FromQuery] CurrencyRequestDTO request)
        {
            try
            {
                var result = await _currencyService.CurrencyCalculateAverageSpreadOnTheDay(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
