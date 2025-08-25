
namespace ExchangeRate.Application.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string URLAwesomeAPI { get; set; } = default!;
        public string ConnectionString { get; set; } = default!;
    }
}
