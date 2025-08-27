using ExchangeRate.Application.DTO.Currency;
using ExchangeRate.Application.DTO.Currency.Request;
using ExchangeRate.Application.DTO.Response;
using ExchangeRate.Application.Interface.Currency;
using ExchangeRate.Domain.Entity.Currency;
using ExchangeRate.Domain.Entity.Currency.Request;
using ExchangeRate.Domain.Interface.Currency;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Application.Service.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(ICurrencyRepository currencyRepository, ILogger<CurrencyService> logger)
        {
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        public async Task<bool> CreateCurrency(CurrencyDTO currencyCreateDTO)
        {
            _logger.LogInformation("Starting the creation of the currency. Code: {Code}, CodeIn: {CodeIn}", currencyCreateDTO.Code, currencyCreateDTO.Codein);

            var result = await _currencyRepository.CreateCurrency(new CurrencyInfo
            {
                Code = currencyCreateDTO.Code,
                Codein = currencyCreateDTO.Codein,
                Bid = currencyCreateDTO.Bid,
                Ask = currencyCreateDTO.Ask,
                DateOfCurrency = currencyCreateDTO.DateOfCurrency,
                CreatedAT = currencyCreateDTO.CreatedAT
            });

            if (!result)
            {
                _logger.LogWarning("Currency already exists. Code: {Code}, CodeIn: {CodeIn}", currencyCreateDTO.Code, currencyCreateDTO.Codein);
                return result;
            }

            _logger.LogInformation("Currency created successfully. Code: {Code}, CodeIn: {CodeIn}", currencyCreateDTO.Code, currencyCreateDTO.Codein);
            return result;
        }

        public async Task<ApiResponse> CurrencyCalculateAverageSpreadOnTheDay(CurrencyRequestDTO request)
        {
            _logger.LogInformation("Calculating average spread for {Code}/{CodeIn} on {Date}", request.Code, request.CodeIn, DateTime.Now.Date);

            var currency = await _currencyRepository.SelectCurrency(new CurrencyRequest
            {
                Code = request.Code,
                CodeIn = request.CodeIn,
                DateOfCurrency = DateTime.Now.Date
            });

            if (!currency.Any())
            {
                _logger.LogWarning("No currency data found for {Code}/{CodeIn} on {Date}", request.Code, request.CodeIn, DateTime.Now.Date);
            }

            var averageSpread = currency.Any() ? Math.Round(currency.Average(c => c.Ask - c.Bid), 4) : 0;

            _logger.LogDebug("Calculated average spread: {Spread}", averageSpread);

            return new ApiResponse
            {
                Data = new CurrencyCalculateAverageSpreadDTO
                {
                    AverageSpread = averageSpread
                },
                Message = "Success",
                Success = true
            };
        }

        public async Task<ApiResponse> CurrencyCalculatePriceBidVariationOnTheDay(CurrencyRequest request)
        {
            request.DateOfCurrency = DateTime.Now.Date;
            _logger.LogInformation("Calculating price bid variation for {Code}/{CodeIn} on {Date}", request.Code, request.CodeIn, request.DateOfCurrency);

            var currency = await _currencyRepository.SelectCurrency(request);

            if (!currency.Any())
            {
                _logger.LogWarning("No currency data found for {Code}/{CodeIn} on {Date}", request.Code, request.CodeIn, request.DateOfCurrency);
                return new ApiResponse
                {
                    Data = new CurrencyPriceBidVariationDTO(),
                    Message = "No data found",
                    Success = false
                };
            }

            var ordered = currency.OrderBy(c => c.DateOfCurrency).ToList();

            decimal firstCurrency = Math.Round(ordered.First().Bid, 4);
            decimal lastCurrency = Math.Round(ordered.Last().Bid, 4);

            decimal variationPrice = Math.Round(lastCurrency - firstCurrency, 4);
            decimal variationPercentage = Math.Round((lastCurrency - firstCurrency) / firstCurrency * 100, 4);

            _logger.LogDebug("First bid: {FirstBid}, Last bid: {LastBid}, Variation: {Variation}, Percentage: {Percentage}%",
                firstCurrency, lastCurrency, variationPrice, variationPercentage);

            _logger.LogInformation("Currency price bid variation calculation completed for {Code}/{CodeIn}", request.Code, request.CodeIn);

            return new ApiResponse
            {
                Data = new CurrencyPriceBidVariationDTO
                {
                    FirstBidPrice = firstCurrency,
                    LastBidPrice = lastCurrency,
                    VariationPrice = variationPrice,
                    VariationPercentage = variationPercentage
                },
                Message = "Success",
                Success = true
            };
        }
    }
}
