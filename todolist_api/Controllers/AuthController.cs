using Microsoft.AspNetCore.Mvc;
using todolist_api.Models;
using todolist_api.Repositories;
using todolist_api.Services;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserLoginRepository _users;
        private readonly IJwtService _jwtService;

        public AuthController(UserLoginRepository users, IJwtService jwtService)
        {
            _users = users;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserAuthDto userDto)
        {
            if(userDto == null || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.PasswordHash))
            {
                return BadRequest("Invalid user data");
            }
            
            try
            {
                var isExist = await _users.IsUserExists(userDto);

                if(isExist)
                {
                    var pass = await _users.IsPasswordCorrect(userDto);

                    if(pass)
                    {
                        var user = await _users.GetUser(userDto.Username);
                        var token = _jwtService.GenerateToken(user.Id);

                        return Ok(new { Token = token });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }


        [HttpPost("registration")]
        public async Task<ActionResult> Registration([FromBody] UserAuthDto userDto)
        {
            if(userDto == null || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.PasswordHash))
            {
                return BadRequest("Invalid user data");
            }

            try
            {
                var isExist = await _users.IsUserExists(userDto);

                if(isExist)
                {
                    return StatusCode(404, new { Token = "User already exist" });
                }

                var user = await _users.AddNewUser(userDto);

                var token = _jwtService.GenerateToken(user.Id);

                return Ok(new { Token = token });
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}