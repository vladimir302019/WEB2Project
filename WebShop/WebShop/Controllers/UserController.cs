using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebShop.DTO.UserDTOs;
using WebShop.Interfaces;

namespace WebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("register")]
        //[Authorize]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDTO)
        {
            UserDTO user = await _userService.Register(registerDTO);
            return Ok(user);
        }
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            TokenDTO token = await _userService.Login(loginDTO);
            return Ok(token);
        }

        [HttpGet("user")]
        // [Authorize]
        public async Task<IActionResult> GetUser([FromQuery] TokenDTO tokenDTO)
        {
            UserDTO userDTO = await _userService.GetUser(tokenDTO.UserId);
            return Ok(userDTO);

        }

        [HttpGet("activated-sellers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActivatedSellers()
        {
            return Ok(await _userService.GetSellers());

        }

        [HttpGet("unactivated-sellers")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetUnactivatedSellers()
        {
            return Ok(await _userService.GetAllUnactivatedSellers());

        }
        //update
        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDTO user)
        {
            return Ok(await _userService.UpdateUser(user.Id, user));

        }
        //activate
        [HttpPut("activate-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser([FromBody] ActivateUserDTO user)
        {
            return Ok(await _userService.ActivateUser(user.Id, user.IsActive));

        }
    }
}
