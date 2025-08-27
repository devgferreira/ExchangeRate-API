using ExchangeRate.Application.API.AwesomeAPI;
using ExchangeRate.Application.DTO.AwesomeAPI;
using ExchangeRate.Application.Interface.AwesomeAPI;
using ExchangeRate.Application.Settings;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate_Test.Service.AwesomeAPI
{
    public class AwesomeAPIServiceTest
    {
        private readonly Mock<IApplicationSettings> _settingsMock;
        private readonly Mock<ILogger<AwesomeAPIService>> _loggerMock;

        public AwesomeAPIServiceTest()
        {
            _settingsMock = new Mock<IApplicationSettings>();
            _loggerMock = new Mock<ILogger<AwesomeAPIService>>();
            _settingsMock.SetupGet(s => s.URLAwesomeAPI).Returns("https://fakeapi.com");
        }

        private IAwesomeAPIService CreateServiceWithHttpResponse(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var client = new HttpClient(handlerMock.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

            return new AwesomeAPIService(httpClientFactoryMock.Object, _settingsMock.Object, _loggerMock.Object);
        }

        #region GetLastCurrencyAsync

        [Fact]
        public async Task GetLastCurrencyAsync_ShouldReturnData_WhenApiReturnsData()
        {
            var fakeData = new Dictionary<string, AwesomeAPIDTO>
            {
                { "USDBRL", new AwesomeAPIDTO { Bid = 5.10m, Ask = 5.20m, Code = "USD", Codein = "BRL" } }
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(fakeData)
            };

            var service = CreateServiceWithHttpResponse(response);

            var result = await service.GetLastCurrencyAsync("USD");

            Assert.NotNull(result);
            Assert.Equal("USD", result.Code);
            Assert.Equal("BRL", result.Codein);
            Assert.Equal(5.10m, result.Bid);
            Assert.Equal(5.20m, result.Ask);
        }

        [Fact]
        public async Task GetLastCurrencyAsync_ShouldThrow_WhenApiReturnsEmpty()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new Dictionary<string, AwesomeAPIDTO>())
            };

            var service = CreateServiceWithHttpResponse(response);

            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetLastCurrencyAsync("USD"));
            Assert.Contains("Exchange rate not found", ex.Message);
        }

        [Fact]
        public async Task GetLastCurrencyAsync_ShouldThrow_WhenHttpRequestFails()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var client = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

            var service = new AwesomeAPIService(httpClientFactoryMock.Object, _settingsMock.Object, _loggerMock.Object);

            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetLastCurrencyAsync("USD"));
            Assert.Contains("Error while communicating with AwesomeAPI", ex.Message);
        }

        [Fact]
        public async Task GetLastCurrencyAsync_ShouldThrow_WhenUnexpectedErrorOccurs()
        {
            // Simula erro inesperado ao tentar ler o JSON
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("invalid-json")
            };

            var service = CreateServiceWithHttpResponse(response);

            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetLastCurrencyAsync("USD"));
            Assert.Contains("Unexpected error while retrieving currency", ex.Message);
        }

        #endregion
    }
}
