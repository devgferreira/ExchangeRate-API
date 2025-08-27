namespace ExchangeRate.Application.Settings
{
    public interface IApplicationSettings
    {
        string URLAwesomeAPI { get; }
        string ConnectionString { get; }
        string JwtSecretKey { get; }
        string JwtIssuer { get; }
        string JwtAudience { get; }
    }
}
