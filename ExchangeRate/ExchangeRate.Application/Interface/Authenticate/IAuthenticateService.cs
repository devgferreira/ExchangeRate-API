namespace ExchangeRate.Application.Interface.Authenticate
{
    public interface IAuthenticateService
    {
        Task<bool> AutheticateAsync(string email, string password);
        public string GenerateToken(string name, string email);

    }
}
