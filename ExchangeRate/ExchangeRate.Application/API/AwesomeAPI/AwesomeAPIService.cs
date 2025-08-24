using ExchangeRate.Application.DTO.Currency;
using ExchangeRate.Application.Interface.AwesomeAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.API.AwesomeAPI
{
    public class AwesomeAPIService : IAwesomeAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AwesomeAPIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CurrencyDTO> GetCurrencyAsync(string coin)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("CambioAPI-ProcessoSeletivo/1.0");
            var url = $"https://economia.awesomeapi.com.br/json/last/{coin}-BRL";
            try
            {
                var response = await client.GetFromJsonAsync<Dictionary<string, CurrencyDTO>>(url);
                if (response == null || response.Count == 0)
                    throw new Exception("Cotação não encontrada.");

                var key = response.First().Key;
                var data = response[key];
                return data;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro ao comunicar com a API de cotações: {ex.Message}");
            }
        }
    }
}
