using ExchangeRate.Application.Service.Authenticate;
using ExchangeRate.Application.Settings;
using ExchangeRate.Domain.Entity.User;
using ExchangeRate.Domain.Entity.User.Request;
using ExchangeRate.Domain.Interface.User;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate_Test.Service.Authenticate
{
    public class AuthenticateServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<AuthenticateService>> _loggerMock;
        private readonly Mock<IApplicationSettings> _settingsMock;
        private readonly AuthenticateService _service;

        public AuthenticateServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<AuthenticateService>>();
            _settingsMock = new Mock<IApplicationSettings>();

            _settingsMock.SetupGet(s => s.JwtSecretKey).Returns("supersecretkey1234567890");
            _settingsMock.SetupGet(s => s.JwtIssuer).Returns("TestIssuer");
            _settingsMock.SetupGet(s => s.JwtAudience).Returns("TestAudience");

            _service = new AuthenticateService(
                _userRepositoryMock.Object,
                _settingsMock.Object,
                _loggerMock.Object
            );
        }

        #region AutheticateAsync

        [Fact]
        public async Task AutheticateAsync_ShouldReturnTrue_WhenPasswordMatches()
        {
            var password = "Strong@123";
            using var hmac = new HMACSHA512();
            var user = new UserInfo
            {
                Email = "test@email.com",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo> { user });

            var result = await _service.AutheticateAsync(user.Email, password);

            Assert.True(result);
        }

        [Fact]
        public async Task AutheticateAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo>());

            var result = await _service.AutheticateAsync("notfound@email.com", "password");

            Assert.False(result);
        }

        [Fact]
        public async Task AutheticateAsync_ShouldReturnFalse_WhenPasswordIncorrect()
        {
            var password = "Strong@123";
            using var hmac = new HMACSHA512();
            var user = new UserInfo
            {
                Email = "test@email.com",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo> { user });

            var result = await _service.AutheticateAsync(user.Email, "WrongPassword");

            Assert.False(result);
        }

        #endregion

        #region GenerateToken

        [Fact]
        public void GenerateToken_ShouldReturnJwtString()
        {
            var settingsMock = new Mock<IApplicationSettings>();
            settingsMock.SetupGet(s => s.JwtSecretKey)
            .Returns("supersecretkey1234567890abcdef1234567890abcdef1234567890abcdef");
            settingsMock.SetupGet(s => s.JwtIssuer).Returns("TestIssuer");
            settingsMock.SetupGet(s => s.JwtAudience).Returns("TestAudience");

            var userRepositoryMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<AuthenticateService>>();

            var service = new AuthenticateService(userRepositoryMock.Object, settingsMock.Object, loggerMock.Object);

            var token = service.GenerateToken("Gabriel", "test@email.com");

            Assert.False(string.IsNullOrWhiteSpace(token));

            // Decodifica JWT para verificar claims
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Contains(jwtToken.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Name && c.Value == "Gabriel");
            Assert.Contains(jwtToken.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Email && c.Value == "test@email.com");
        }

        #endregion
    }
}
