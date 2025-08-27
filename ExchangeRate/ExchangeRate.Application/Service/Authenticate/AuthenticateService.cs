using ExchangeRate.Application.Interface.Authenticate;
using ExchangeRate.Application.Settings;
using ExchangeRate.Domain.Entity.User.Request;
using ExchangeRate.Domain.Interface.User;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExchangeRate.Application.Service.Authenticate
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApplicationSettings _configuration;
        private readonly ILogger<AuthenticateService> _logger;

        public AuthenticateService(IUserRepository userRepository, IApplicationSettings configuration, ILogger<AuthenticateService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> AutheticateAsync(string email, string password)
        {
            _logger.LogInformation("Authentication attempt for email: {Email}", email);

            var users = await _userRepository.SelectUser(new UserRequest { Email = email });

            var user = users.FirstOrDefault();
            if (user == null)
            {
                _logger.LogWarning("Authentication failed: user not found. Email: {Email}", email);
                return false;
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int x = 0; x < computedHash.Length; x++)
            {
                if (computedHash[x] != user.PasswordHash[x])
                {
                    _logger.LogWarning("Authentication failed: invalid password for email: {Email}", email);
                    return false;
                }
            }

            _logger.LogInformation("Authentication successful for email: {Email}", email);
            return true;
        }

        public string GenerateToken(string name, string email)
        {
            _logger.LogInformation("Generating JWT token for user: {Email}", email);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JwtSecretKey));
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(10);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration.JwtIssuer,
                audience: _configuration.JwtAudience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogDebug("JWT generated with expiration: {Expiration}", expiration);
            _logger.LogInformation("JWT token successfully generated for user: {Email}", email);

            return tokenString;
        }
    }
}
