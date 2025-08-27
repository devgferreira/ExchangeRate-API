using ExchangeRate.Application.DTO.User;
using ExchangeRate.Application.Interface.User;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                await _userService.CreateUser(userDTO);
                return Ok(new { message = "User created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                var token = await _userService.Login(userLoginDTO);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
