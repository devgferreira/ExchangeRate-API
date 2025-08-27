using ExchangeRate.Application.DTO.Currency;
using ExchangeRate.Application.DTO.Currency.Request;
using ExchangeRate.Application.DTO.Response;
using ExchangeRate.Domain.Entity.Currency.Request;

namespace ExchangeRate.Application.Interface.Currency
{
    public interface ICurrencyService
    {
        Task<bool> CreateCurrency(CurrencyDTO currencyCreateDTO);
        Task<ApiResponse> CurrencyCalculatePriceBidVariationOnTheDay(CurrencyRequest request);
        Task<ApiResponse> CurrencyCalculateAverageSpreadOnTheDay(CurrencyRequestDTO request);
    }
}
