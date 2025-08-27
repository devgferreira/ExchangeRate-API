using ExchangeRate.Application.DTO.AwesomeAPI;
using ExchangeRate.Application.Interface.AwesomeAPI;
using ExchangeRate.Application.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRate.Application.API.AwesomeAPI
{
    public class AwesomeAPIService : IAwesomeAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApplicationSettings _applicationSettings;
        private readonly ILogger<AwesomeAPIService> _logger;

        public AwesomeAPIService(IHttpClientFactory httpClientFactory, IApplicationSettings applicationSettings, ILogger<AwesomeAPIService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _applicationSettings = applicationSettings;
            _logger = logger;
        }

        public async Task<AwesomeAPIDTO> GetLastCurrencyAsync(string currency)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("ExchangeRate.API/1.0");

            var url = $"{_applicationSettings.URLAwesomeAPI}/last/{currency}-BRL";

            _logger.LogInformation("Requesting last currency quote for {Currency} from AwesomeAPI. URL: {Url}", currency, url);

            try
            {
                var response = await client.GetFromJsonAsync<Dictionary<string, AwesomeAPIDTO>>(url);

                if (response == null || response.Count == 0)
                {
                    _logger.LogWarning("No exchange rate found for {Currency}", currency);
                    throw new Exception("Exchange rate not found.");
                }

                var key = response.First().Key;
                var data = response[key];

                _logger.LogInformation("Successfully retrieved currency data for {Currency}", currency);
                _logger.LogDebug("Currency data: {@Data}", data);

                return data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while communicating with AwesomeAPI for currency {Currency}", currency);
                throw new Exception($"Error while communicating with AwesomeAPI: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving currency {Currency} from AwesomeAPI", currency);
                throw new Exception($"Unexpected error while retrieving currency {currency}: {ex.Message}");
            }
        }
    }
}
