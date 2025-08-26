using ExchangeRate.Application.DTO.Currency;
using ExchangeRate.Application.DTO.Currency.Request;
using ExchangeRate.Domain.Entity.Currency.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Interface.Currency
{
    public interface ICurrencyService
    {
        Task<bool> CreateCurrency(CurrencyDTO currencyCreateDTO);
        Task<CurrencyPriceBidVariationDTO> CurrencyCalculatePriceBidVariationOnTheDay(CurrencyRequest request);
        Task<CurrencyCalculateAverageSpreadDTO> CurrencyCalculateAverageSpreadOnTheDay(CurrencyRequestDTO request);
    }
}
