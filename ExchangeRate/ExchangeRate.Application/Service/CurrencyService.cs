using ExchangeRate.Application.DTO.Currency;
using ExchangeRate.Application.DTO.Currency.Request;
using ExchangeRate.Application.DTO.Response;
using ExchangeRate.Application.Interface.Currency;
using ExchangeRate.Domain.Entity.Currency;
using ExchangeRate.Domain.Entity.Currency.Request;
using ExchangeRate.Domain.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Service
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
            _logger.LogInformation("Starting the creation of the currency.");
            var reuslt = await _currencyRepository.CreateCurrency(new CurrencyInfo
            {
                Code  = currencyCreateDTO.Code,
                Codein = currencyCreateDTO.Codein,
                Bid = currencyCreateDTO.Bid,
                Ask = currencyCreateDTO.Ask,
                DateOfCurrency = currencyCreateDTO.DateOfCurrency,
                CreatedAT = currencyCreateDTO.CreatedAT

            });
            if (!reuslt)
            {
                _logger.LogWarning("Currency already exists.");
                return reuslt;
            }
            _logger.LogInformation("Currency created successfully.");

            return reuslt;

        }

        public async Task<ApiResponse> CurrencyCalculateAverageSpreadOnTheDay(CurrencyRequestDTO request)
        {
            var currency = await _currencyRepository.SelectCurrency(new CurrencyRequest
            {
                Code = request.Code,
                CodeIn = request.CodeIn,
                DateOfCurrency = DateTime.Now.Date
            });

            return new ApiResponse
            {
                Data = new CurrencyCalculateAverageSpreadDTO
                {
                    AverageSpread = currency.Any() ? Math.Round(currency.Average(c => c.Ask - c.Bid), 4) : 0
                },
                Message = "Success",
                Success = true
            };
        }

        public async Task<ApiResponse> CurrencyCalculatePriceBidVariationOnTheDay(CurrencyRequest request)
        {
            request.DateOfCurrency = DateTime.Now.Date;
            var currency = await _currencyRepository.SelectCurrency(request);

            var ordered = currency.OrderBy(c => c.DateOfCurrency).ToList();

            decimal firstCurrency = Math.Round(ordered.First().Bid, 4);
            decimal lastCurrency = Math.Round(ordered.Last().Bid, 4);

            decimal variationPrice = Math.Round(lastCurrency - firstCurrency, 4);
            decimal variationPercentage = Math.Round(((lastCurrency - firstCurrency) / firstCurrency) * 100, 4);
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
