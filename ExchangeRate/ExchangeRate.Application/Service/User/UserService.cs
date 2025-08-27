using ExchangeRate.Application.DTO.Token;
using ExchangeRate.Application.DTO.User;
using ExchangeRate.Application.Interface.Authenticate;
using ExchangeRate.Application.Interface.User;
using ExchangeRate.Domain.Entity.User;
using ExchangeRate.Domain.Entity.User.Request;
using ExchangeRate.Domain.Interface.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Service.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticateService _authenticateService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IAuthenticateService authenticateService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _authenticateService = authenticateService;
            _logger = logger;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            _logger.LogInformation("Starting user creation for email: {Email}", userDTO.Email);

            await ValidateUserRegister(userDTO);

            using var hmac = new HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
            byte[] passwordSalt = hmac.Key;

            await _userRepository.CreateUser(new UserInfo
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now,
            });

            _logger.LogInformation("User created successfully: {Email}", userDTO.Email);
        }

        public async Task<TokenDTO> Login(UserLoginDTO userLoginDTO)
        {
            _logger.LogInformation("Login attempt for email: {Email}", userLoginDTO.Email);

            var userValidate = await ValidateUserLogin(userLoginDTO);

            var token = _authenticateService.GenerateToken(userValidate.Name, userValidate.Email);

            _logger.LogInformation("Login successful for user: {Email}", userValidate.Email);

            return new TokenDTO
            {
                AccessToken = token,
            };
        }

        #region Validations

        private async Task<UserInfo> ValidateUserLogin(UserLoginDTO userLoginDTo)
        {
            ValidateLoginFields(userLoginDTo);

            var user = await ValidateUserNotFound(userLoginDTo.Email);

            var isAuthenticated = await _authenticateService.AutheticateAsync(user.Email, userLoginDTo.Password);

            if (!isAuthenticated)
            {
                _logger.LogWarning("Authentication failed for email: {Email}", userLoginDTo.Email);
                throw new Exception("Invalid credentials.");
            }

            return user;
        }

        private void ValidateLoginFields(UserLoginDTO userLoginDTo)
        {
            if (string.IsNullOrWhiteSpace(userLoginDTo.Email) || string.IsNullOrWhiteSpace(userLoginDTo.Password))
            {
                _logger.LogWarning("Login validation failed: empty email or password.");
                throw new Exception("Please, fill in all fields.");
            }
        }

        private async Task ValidateUserRegister(UserDTO userDTo)
        {
            ValidateRegisterFields(userDTo);

            ValidadePasswordMatch(userDTo.Password, userDTo.ConfirmPassword);

            if (!IsPasswordStrong(userDTo.Password))
            {
                _logger.LogWarning("Weak password provided for email: {Email}", userDTo.Email);
                throw new Exception("The password must have at least 6 characters and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
            }

            var userExists = await _userRepository.SelectUser(new UserRequest { Email = userDTo.Email });
            if (userExists.Any())
            {
                _logger.LogWarning("User already exists: {Email}", userDTo.Email);
                throw new Exception("The user already exists.");
            }
        }

        private void ValidadePasswordMatch(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                _logger.LogWarning("Password mismatch on registration attempt.");
                throw new Exception("Password does not match.");
            }
        }

        private void ValidateRegisterFields(UserDTO userDTo)
        {
            if (string.IsNullOrWhiteSpace(userDTo.Email) ||
                string.IsNullOrWhiteSpace(userDTo.Password) ||
                string.IsNullOrWhiteSpace(userDTo.ConfirmPassword) ||
                string.IsNullOrWhiteSpace(userDTo.Name))
            {
                _logger.LogWarning("Registration validation failed: missing required fields.");
                throw new Exception("Please, fill in all fields.");
            }
        }

        private async Task<UserInfo> ValidateUserNotFound(string email)
        {
            var user = await _userRepository.SelectUser(new UserRequest { Email = email });
            if (!user.Any())
            {
                _logger.LogWarning("User not found with email: {Email}", email);
                throw new Exception("User not found.");
            }

            return user.FirstOrDefault();
        }

        private bool IsPasswordStrong(string password)
        {
            bool result = password.Length >= 6 &&
                          Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$");

            if (!result)
                _logger.LogDebug("Password strength validation failed.");

            return result;
        }

        #endregion
    }
}
