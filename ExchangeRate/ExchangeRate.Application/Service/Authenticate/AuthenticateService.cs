using ExchangeRate.Application.Interface.Authenticate;
using ExchangeRate.Application.Settings;
using ExchangeRate.Domain.Entity.User.Request;
using ExchangeRate.Domain.Interface.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Service.Authenticate
{
    public class AuthenticateService : IAuthenticateService
    {

        private readonly IUserRepository _userRepository;
        private readonly IApplicationSettings _configuration;

        public AuthenticateService(IUserRepository userRepository, IApplicationSettings configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<bool> AutheticateAsync(string email, string password)
        {
            var users = await _userRepository.SelectUser(new UserRequest { Email = email });

            if (users.FirstOrDefault() == null)
                return false;

            using var hmac = new HMACSHA512(users.FirstOrDefault().PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int x = 0; x < computedHash.Length; x++)
            {
                if (computedHash[x] != users.FirstOrDefault().PasswordHash[x])
                    return false;
            }

            return true;
        }

        public string GenerateToken(string name, string email)
        {
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
