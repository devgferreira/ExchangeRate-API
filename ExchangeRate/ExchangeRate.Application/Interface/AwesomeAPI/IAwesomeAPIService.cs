using ExchangeRate.Application.DTO.AwesomeAPI;

namespace ExchangeRate.Application.Interface.AwesomeAPI
{
    public interface IAwesomeAPIService
    {
        Task<AwesomeAPIDTO> GetLastCurrencyAsync(string currency);
    }
}
