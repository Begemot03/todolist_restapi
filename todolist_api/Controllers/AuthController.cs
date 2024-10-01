using System.ComponentModel.DataAnnotations;
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
        public async Task<ActionResult> Login([Required][FromBody] UserAuthDto userDto)
        {
            try
            {
                var user = await _users.GetUser(userDto.Username);

                if(user == null)
                {
                    return Unauthorized();
                }

                var isPasswordCorrect = await _users.IsDataCorrect(userDto);

                if(!isPasswordCorrect)
                {
                    return Unauthorized();
                }

                var token = _jwtService.GenerateToken(user.Id);

                return Ok(token);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }


        [HttpPost("registration")]
        public async Task<ActionResult> Registration([Required][FromBody] UserAuthDto userDto)
        {
            try
            {
                var isExist = await _users.IsUserExists(userDto);

                if(isExist)
                {
                    return Conflict(new { message = "User already exists" }); 
                }

                var user = await _users.AddNewUser(userDto);

                var token = _jwtService.GenerateToken(user.Id);

                return Ok(token);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}