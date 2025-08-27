using ExchangeRate.Application.DTO.Token;
using ExchangeRate.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Interface.User
{
    public interface IUserService
    {
        Task CreateUser(UserDTO userDTO);
        Task<TokenDTO> Login(UserLoginDTO userLoginDTO);
    }
}
