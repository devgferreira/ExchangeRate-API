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
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Iniciando o processo. {DateTime.Now}");
                    using var scope = _scopeFactory.CreateScope();

                    var awesomeAPIService = scope.ServiceProvider.GetRequiredService<IAwesomeAPIService>();
                    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

                    var result = await awesomeAPIService.GetLastCurrencyAsync("USD");

                    await currencyService.CreateCurrency(new CurrencyDTO
                    {
                        Symbol = result.Code,
                        Bid = result.Bid,
                        Ask = result.Ask,
                        DateOfCurrency = result.Create_date.ToString(),
                        CreatedAT = DateTime.UtcNow
                    });
                    _logger.LogInformation($"Woker finalizado {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao coletar cotações");
                }

                await Task.Delay(TimeSpan.FromSeconds(63), stoppingToken);
            }
        }
    }
}
