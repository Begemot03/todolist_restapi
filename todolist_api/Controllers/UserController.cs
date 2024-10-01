using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolist_api.Database;
using todolist_api.Models;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController(ApplicationContext context) : ControllerBase
    {
        private readonly ApplicationContext Context = context;

        [HttpGet("me")]
        public async Task<ActionResult> GetUser()
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == userId);

                if(user == null)
                {
                    return NotFound();
                }

                var userDto = new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username
                };

                return Ok(userDto);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}