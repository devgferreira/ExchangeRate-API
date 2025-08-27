using ExchangeRate.Application.DTO.Token;
using ExchangeRate.Application.DTO.User;

namespace ExchangeRate.Application.Interface.User
{
    public interface IUserService
    {
        Task CreateUser(UserDTO userDTO);
        Task<TokenDTO> Login(UserLoginDTO userLoginDTO);
    }
}
