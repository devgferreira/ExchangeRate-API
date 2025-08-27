using ExchangeRate.Application.DTO.Currency;
using ExchangeRate.Application.DTO.Currency.Request;
using ExchangeRate.Application.Service.Currency;
using ExchangeRate.Domain.Entity.Currency;
using ExchangeRate.Domain.Entity.Currency.Request;
using ExchangeRate.Domain.Interface.Currency;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate_Test.Service.Currency
{
    public class CurrencyServiceTest
    {
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
        private readonly Mock<ILogger<CurrencyService>> _loggerMock;
        private readonly CurrencyService _currencyService;

        public CurrencyServiceTest()
        {
            _currencyRepositoryMock = new Mock<ICurrencyRepository>();
            _loggerMock = new Mock<ILogger<CurrencyService>>();
            _currencyService = new CurrencyService(_currencyRepositoryMock.Object, _loggerMock.Object);
        }

        #region CreateCurrency

        [Fact]
        public async Task CreateCurrency_ShouldReturnTrue_WhenCurrencyDoesNotExist()
        {
            var dto = new CurrencyDTO
            {
                Code = "USD",
                Codein = "BRL",
                Bid = 5.10m,
                Ask = 5.20m,
                DateOfCurrency = DateTime.Now.Date,
                CreatedAT = DateTime.Now
            };

            _currencyRepositoryMock.Setup(r => r.CreateCurrency(It.IsAny<CurrencyInfo>()))
                                   .ReturnsAsync(true);

            var result = await _currencyService.CreateCurrency(dto);

            Assert.True(result);
            _currencyRepositoryMock.Verify(r => r.CreateCurrency(It.Is<CurrencyInfo>(c => c.Code == "USD" && c.Codein == "BRL")), Times.Once);
        }

        [Fact]
        public async Task CreateCurrency_ShouldReturnFalse_WhenCurrencyAlreadyExists()
        {
            var dto = new CurrencyDTO
            {
                Code = "USD",
                Codein = "BRL",
                Bid = 5.10m,
                Ask = 5.20m,
                DateOfCurrency = DateTime.Now.Date,
                CreatedAT = DateTime.Now
            };

            _currencyRepositoryMock.Setup(r => r.CreateCurrency(It.IsAny<CurrencyInfo>()))
                                   .ReturnsAsync(false);

            var result = await _currencyService.CreateCurrency(dto);

            Assert.False(result);
        }

        #endregion

        #region CurrencyCalculateAverageSpreadOnTheDay

        [Fact]
        public async Task CurrencyCalculateAverageSpreadOnTheDay_ShouldReturnAverage_WhenDataExists()
        {
            var request = new CurrencyRequestDTO { Code = "USD", CodeIn = "BRL" };

            var currencies = new List<CurrencyInfo>
            {
                new CurrencyInfo { Bid = 5.10m, Ask = 5.20m },
                new CurrencyInfo { Bid = 5.15m, Ask = 5.25m }
            };

            _currencyRepositoryMock.Setup(r => r.SelectCurrency(It.IsAny<CurrencyRequest>()))
                                   .ReturnsAsync(currencies);

            var response = await _currencyService.CurrencyCalculateAverageSpreadOnTheDay(request);

            Assert.True(response.Success);
            Assert.Equal(0.10m, ((CurrencyCalculateAverageSpreadDTO)response.Data).AverageSpread);
        }

        [Fact]
        public async Task CurrencyCalculateAverageSpreadOnTheDay_ShouldReturnZero_WhenNoDataExists()
        {
            var request = new CurrencyRequestDTO { Code = "USD", CodeIn = "BRL" };

            _currencyRepositoryMock.Setup(r => r.SelectCurrency(It.IsAny<CurrencyRequest>()))
                                   .ReturnsAsync(new List<CurrencyInfo>());

            var response = await _currencyService.CurrencyCalculateAverageSpreadOnTheDay(request);

            Assert.True(response.Success);
            Assert.Equal(0m, ((CurrencyCalculateAverageSpreadDTO)response.Data).AverageSpread);
        }

        #endregion

        #region CurrencyCalculatePriceBidVariationOnTheDay

        [Fact]
        public async Task CurrencyCalculatePriceBidVariationOnTheDay_ShouldReturnVariation_WhenDataExists()
        {
            var request = new CurrencyRequest { Code = "USD", CodeIn = "BRL" };

            var currencies = new List<CurrencyInfo>
            {
                new CurrencyInfo { Bid = 5.10m, DateOfCurrency = DateTime.Now },
                new CurrencyInfo { Bid = 5.20m, DateOfCurrency = DateTime.Now }
            };

            _currencyRepositoryMock.Setup(r => r.SelectCurrency(It.IsAny<CurrencyRequest>()))
                                   .ReturnsAsync(currencies);

            var response = await _currencyService.CurrencyCalculatePriceBidVariationOnTheDay(request);

            var data = (CurrencyPriceBidVariationDTO)response.Data;

            Assert.True(response.Success);
            Assert.Equal(5.10m, data.FirstBidPrice);
            Assert.Equal(5.20m, data.LastBidPrice);
            Assert.Equal(0.10m, data.VariationPrice);
            Assert.Equal(1.9608m, data.VariationPercentage);
        }

        [Fact]
        public async Task CurrencyCalculatePriceBidVariationOnTheDay_ShouldReturnNoData_WhenDataDoesNotExist()
        {
            var request = new CurrencyRequest { Code = "USD", CodeIn = "BRL" };

            _currencyRepositoryMock.Setup(r => r.SelectCurrency(It.IsAny<CurrencyRequest>()))
                                   .ReturnsAsync(new List<CurrencyInfo>());

            var response = await _currencyService.CurrencyCalculatePriceBidVariationOnTheDay(request);

            Assert.False(response.Success);
            Assert.IsType<CurrencyPriceBidVariationDTO>(response.Data);
        }

        #endregion
    }
}
