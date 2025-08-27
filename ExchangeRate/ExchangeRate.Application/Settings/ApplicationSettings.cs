
namespace ExchangeRate.Application.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string URLAwesomeAPI { get; set; } = default!;
        public string ConnectionString { get; set; } = default!;
        public string JwtSecretKey { get; set; } = default!;
        public string JwtIssuer { get; set; } = default!;
        public string JwtAudience { get; set; } = default!;
    }
}
