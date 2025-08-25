using ExchangeRate.Infra.IoC;
using ExchangeRate.Worker.CurrencyRates;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddInfrastructureAPI(builder.Configuration);
var host = builder.Build();
host.Run();
