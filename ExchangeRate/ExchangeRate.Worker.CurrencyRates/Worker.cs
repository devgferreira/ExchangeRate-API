using ExchangeRate.Application.DTO.Currency;
using ExchangeRate.Application.Interface.AwesomeAPI;
using ExchangeRate.Application.Interface.Currency;

namespace ExchangeRate.Worker.CurrencyRates
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting the worker CurrencyUSD: {DateTime.Now}");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Starting the process: {DateTime.Now}");

                    using var scope = _scopeFactory.CreateScope();

                    var awesomeAPIService = scope.ServiceProvider.GetRequiredService<IAwesomeAPIService>();
                    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

                    var result = await awesomeAPIService.GetLastCurrencyAsync("USD");

                    await currencyService.CreateCurrency(new CurrencyDTO
                    {
                        Code = result.Code,
                        Codein = result.Codein,
                        Bid = result.Bid,
                        Ask = result.Ask,
                        DateOfCurrency = DateTime.Parse(result.Create_date),
                        CreatedAT = DateTime.UtcNow
                    });
                    _logger.LogInformation($"Finalizing the process: {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao coletar cotações");
                }

                await Task.Delay(TimeSpan.FromSeconds(63), stoppingToken);
            }
            _logger.LogInformation($"Finishing the worker CurrencyUSD: {DateTime.Now}");

        }
    }
}
