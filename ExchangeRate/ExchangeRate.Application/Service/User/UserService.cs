using ExchangeRate.Application.DTO.Token;
using ExchangeRate.Application.DTO.User;
using ExchangeRate.Application.Interface.User;
using ExchangeRate.Domain.Entity.User;
using ExchangeRate.Domain.Entity.User.Request;
using ExchangeRate.Domain.Interface.User;
using Microsoft.AspNetCore.Http;
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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
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
        }

        public Task<TokenDTO> Login(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }

        #region Validations
        private async Task ValidateUserRegister(UserDTO userDTo)
        {
            ValidateRegisterFields(userDTo);

            ValidadePasswordMatch(userDTo.Password, userDTo.ConfirmPassword);

            if (!IsPasswordStrong(userDTo.Password))
            {
                throw new Exception("The password must have at least 6 characters and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
            }

            var userExists = await _userRepository.SelectUser(new UserRequest { Email = userDTo.Email });
            if (userExists.Any())
            {
                throw new Exception("The user already exists.");
            }
        }
        private void ValidadePasswordMatch(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
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
                throw new Exception("Please, fill in all fields.");
            }
        }
        private bool IsPasswordStrong(string password)
        {
            return password.Length >= 6 &&
                   Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$");
        }
        #endregion
    }
}
