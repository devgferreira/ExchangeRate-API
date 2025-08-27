using ExchangeRate.Application.DTO.User;
using ExchangeRate.Application.Interface.Authenticate;
using ExchangeRate.Application.Service.User;
using ExchangeRate.Domain.Entity.User;
using ExchangeRate.Domain.Entity.User.Request;
using ExchangeRate.Domain.Interface.User;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate_Test.Service.User
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticateService> _authServiceMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authServiceMock = new Mock<IAuthenticateService>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _authServiceMock.Object,
                _loggerMock.Object
            );
        }

        #region CreateUser

        [Fact]
        public async Task CreateUser_ShouldCreate_WhenValidData()
        {
            var dto = new UserDTO
            {
                Name = "Gabriel",
                Email = "test@email.com",
                Password = "Strong@123",
                ConfirmPassword = "Strong@123"
            };

            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo>());

            await _userService.CreateUser(dto);

            _userRepositoryMock.Verify(r => r.CreateUser(It.Is<UserInfo>(u => u.Email == dto.Email)), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ShouldThrow_WhenFieldsMissing()
        {
            var dto = new UserDTO { Email = "", Password = "", ConfirmPassword = "", Name = "" };

            await Assert.ThrowsAsync<Exception>(() => _userService.CreateUser(dto));
        }

        [Fact]
        public async Task CreateUser_ShouldThrow_WhenPasswordsDoNotMatch()
        {
            var dto = new UserDTO
            {
                Name = "Gabriel",
                Email = "test@email.com",
                Password = "Strong@123",
                ConfirmPassword = "Wrong@123"
            };

            await Assert.ThrowsAsync<Exception>(() => _userService.CreateUser(dto));
        }

        [Fact]
        public async Task CreateUser_ShouldThrow_WhenPasswordIsWeak()
        {
            var dto = new UserDTO
            {
                Name = "Gabriel",
                Email = "test@email.com",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            await Assert.ThrowsAsync<Exception>(() => _userService.CreateUser(dto));
        }

        [Fact]
        public async Task CreateUser_ShouldThrow_WhenUserAlreadyExists()
        {
            var dto = new UserDTO
            {
                Name = "Gabriel",
                Email = "test@email.com",
                Password = "Strong@123",
                ConfirmPassword = "Strong@123"
            };

            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo> { new UserInfo { Email = dto.Email } });

            await Assert.ThrowsAsync<Exception>(() => _userService.CreateUser(dto));
        }

        #endregion

        #region Login

        [Fact]
        public async Task Login_ShouldReturnToken_WhenValidCredentials()
        {
            var loginDto = new UserLoginDTO { Email = "test@email.com", Password = "Strong@123" };

            var user = new UserInfo { Name = "Gabriel", Email = loginDto.Email };

            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo> { user });

            _authServiceMock.Setup(a => a.AutheticateAsync(loginDto.Email, loginDto.Password))
                            .ReturnsAsync(true);

            _authServiceMock.Setup(a => a.GenerateToken(user.Name, user.Email))
                            .Returns("fake-token");

            var result = await _userService.Login(loginDto);

            Assert.NotNull(result);
            Assert.Equal("fake-token", result.AccessToken);
        }

        [Fact]
        public async Task Login_ShouldThrow_WhenFieldsMissing()
        {
            var loginDto = new UserLoginDTO { Email = "", Password = "" };

            await Assert.ThrowsAsync<Exception>(() => _userService.Login(loginDto));
        }

        [Fact]
        public async Task Login_ShouldThrow_WhenUserNotFound()
        {
            var loginDto = new UserLoginDTO { Email = "notfound@email.com", Password = "Strong@123" };

            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo>());

            await Assert.ThrowsAsync<Exception>(() => _userService.Login(loginDto));
        }

        [Fact]
        public async Task Login_ShouldThrow_WhenAuthenticationFails()
        {
            var loginDto = new UserLoginDTO { Email = "test@email.com", Password = "WrongPass" };

            var user = new UserInfo { Name = "Gabriel", Email = loginDto.Email };

            _userRepositoryMock.Setup(r => r.SelectUser(It.IsAny<UserRequest>()))
                               .ReturnsAsync(new List<UserInfo> { user });

            _authServiceMock.Setup(a => a.AutheticateAsync(loginDto.Email, loginDto.Password))
                            .ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => _userService.Login(loginDto));
        }

        #endregion
    }
}
