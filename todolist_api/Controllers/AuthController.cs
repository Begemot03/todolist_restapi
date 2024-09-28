using Microsoft.AspNetCore.Mvc;
using todolist_api.Models;
using todolist_api.Repositories;
using todolist_api.Services;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserLoginRepository users, JwtService jwtService) : ControllerBase
    {
        private readonly UserLoginRepository Users = users;
        private readonly JwtService JwtService = jwtService;

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserAuthDto userDto)
        {
            if(userDto == null || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.PasswordHash))
            {
                return BadRequest("Invalid user data");
            }
            
            try
            {
                var isExist = await Users.IsUserExists(userDto);

                if(isExist)
                {
                    var pass = await Users.IsPasswordCorrect(userDto);

                    if(pass)
                    {
                        var token = JwtService.GenerateToken(new User()
                        {
                            Username = userDto.Username,
                            PasswordHash = userDto.PasswordHash
                        });

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
                var isExist = await Users.IsUserExists(userDto);

                if(isExist)
                {
                    return StatusCode(404, new { Token = "User already exist" });
                }

                var user = await Users.AddNewUser(userDto);

                var token = JwtService.GenerateToken(user);

                return Ok(new { Token = token });
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}